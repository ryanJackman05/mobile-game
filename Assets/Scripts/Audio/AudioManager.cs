using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Persistent audio, created once in the Boot scene. Call from anywhere:
//   AudioManager.Play(Sfx.ZombieRunOver);
//   AudioManager.SetEngine(running, Mathf.Abs(GameManager.currentSpeed));
//   AudioManager.SetSkid(0..1);
// Every call is a safe no-op when there's no AudioManager (e.g. pressing Play in the Game scene without Boot).
//
// Inspector: the Sfx list gets one row per Sfx value automatically; drag one or more clips into each row
// (several clips = random variation). Music is chosen per scene by name. No AudioMixer needed; volumes are
// player settings saved in PlayerPrefs, like Haptics.
public enum Sfx
{
    UiClick, UpgradeBought, CoinEarned,
    ZombieNotice, ZombieLeap, ZombieLand, ZombieClaw, ZombieKnockedOff, ZombieRunOver,
    CarScrape, SlowArea, Crash, Overrun,
    RepairPickup, GameOverSting,
}

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class SfxEntry
    {
        [HideInInspector] public string name;   // shows the Sfx name as the row label
        public Sfx id;
        public AudioClip[] clips;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 0.5f)] public float pitchJitter = 0.05f;
        [Tooltip("Ignore repeats closer together than this (s). Stops 10 zombies groaning on the same frame.")]
        public float minInterval = 0.05f;
        [Tooltip("Keeps playing while the game is paused (menu sounds).")]
        public bool playWhilePaused;
    }

    [Serializable]
    public class SceneMusic
    {
        public string scene;
        public AudioClip clip;
    }

    [SerializeField] SfxEntry[] sfx = Array.Empty<SfxEntry>();
    [SerializeField] int voices = 8; // simultaneous one-shots

    [Header("Music")]
    [SerializeField] SceneMusic[] music = Array.Empty<SceneMusic>();
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.6f;
    [SerializeField] float musicFadeTime = 0.8f;

    [Header("Car loops")]
    [SerializeField] AudioClip engineLoop;      // e.g. PROMETEO/Sounds/CarEngine.wav
    [SerializeField, Range(0f, 1f)] float engineVolume = 0.5f;
    [SerializeField] Vector2 enginePitch = new Vector2(0.8f, 1.6f);
    [SerializeField] float engineTopSpeed = 40f; // world speed that reaches the top pitch
    [SerializeField] AudioClip skidLoop;        // e.g. PROMETEO/Sounds/TireScreech.wav
    [SerializeField, Range(0f, 1f)] float skidVolume = 0.4f;
    [SerializeField] float loopFadeSpeed = 4f;  // volume per second

    public static AudioManager Instance { get; private set; }

    // ---- player settings ----
    const string MusicKey = "musicVolume", SfxKey = "sfxVolume";
    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicKey, 1f);
        set => PlayerPrefs.SetFloat(MusicKey, Mathf.Clamp01(value)); // saved when the slider closes, not every drag step
    }
    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat(SfxKey, 1f);
        set => PlayerPrefs.SetFloat(SfxKey, Mathf.Clamp01(value));
    }

    // ---- static API ----
    public static void Play(Sfx id) { if (Instance != null) Instance.PlaySfx(id); }
    public static void SetEngine(bool running, float speed) { if (Instance != null) { Instance.engineOn = running; Instance.engineSpeed = speed; } }
    public static void SetSkid(float amount01) { if (Instance != null) Instance.skidAmount = Mathf.Clamp01(amount01); }

    // ---- internals ----
    SfxEntry[] byId;
    float[] lastPlayed;
    AudioSource[] voicePool, pausedVoicePool;
    int nextVoice, nextPausedVoice;
    AudioSource musicSource, engineSource, skidSource;
    AudioClip pendingMusic;
    bool musicSwitching;
    float musicFade; // 0..1
    bool engineOn;
    float engineSpeed, skidAmount;
    float cachedSfxVol = 1f, cachedMusicVol = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        int count = Enum.GetValues(typeof(Sfx)).Length;
        byId = new SfxEntry[count];
        lastPlayed = new float[count];
        for (int i = 0; i < count; i++) lastPlayed[i] = -100f;
        foreach (var e in sfx) if ((int)e.id < count) byId[(int)e.id] = e;

        voicePool = MakeSources(voices, false);
        pausedVoicePool = MakeSources(2, true);
        musicSource = MakeSource(true, false);
        engineSource = MakeSource(true, false);
        skidSource = MakeSource(true, false);
        engineSource.clip = engineLoop;
        skidSource.clip = skidLoop;

        RefreshVolumes();
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void Start() => OnSceneChanged(default, SceneManager.GetActiveScene());

    void OnDestroy()
    {
        if (Instance == this) SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    AudioSource[] MakeSources(int n, bool ignorePause)
    {
        var arr = new AudioSource[n];
        for (int i = 0; i < n; i++) arr[i] = MakeSource(false, ignorePause);
        return arr;
    }

    AudioSource MakeSource(bool loop, bool ignorePause)
    {
        var s = gameObject.AddComponent<AudioSource>();
        s.playOnAwake = false;
        s.loop = loop;
        s.spatialBlend = 0f; // 2D: fixed camera, no need for 3D falloff
        s.ignoreListenerPause = ignorePause;
        s.volume = 0f; // AudioSource defaults to 1, which made the engine/skid loops fade *out* audibly at startup
        return s;
    }

    // Settings sliders call this after changing MusicVolume / SfxVolume
    public static void RefreshVolumes()
    {
        if (Instance == null) return;
        Instance.cachedSfxVol = SfxVolume;
        Instance.cachedMusicVol = MusicVolume;
    }

    void PlaySfx(Sfx id)
    {
        var e = byId[(int)id];
        if (e == null || e.clips == null || e.clips.Length == 0) return; // no clip assigned yet: stay silent

        float now = Time.unscaledTime;
        if (now - lastPlayed[(int)id] < e.minInterval) return;
        lastPlayed[(int)id] = now;

        var clip = e.clips[UnityEngine.Random.Range(0, e.clips.Length)];
        if (clip == null) return;

        AudioSource s;
        if (e.playWhilePaused) { s = pausedVoicePool[nextPausedVoice]; nextPausedVoice = (nextPausedVoice + 1) % pausedVoicePool.Length; }
        else { s = voicePool[nextVoice]; nextVoice = (nextVoice + 1) % voicePool.Length; } // oldest voice gets cut if all are busy

        s.clip = clip;
        s.volume = e.volume * cachedSfxVol;
        s.pitch = 1f + UnityEngine.Random.Range(-e.pitchJitter, e.pitchJitter);
        s.Play();
    }

    void OnSceneChanged(Scene _, Scene next)
    {
        AudioClip clip = null;
        foreach (var m in music) if (m.scene == next.name) { clip = m.clip; break; }
        if (clip == musicSource.clip && musicSource.isPlaying) return;
        pendingMusic = clip;
        musicSwitching = true;

        // leaving the Game scene: the car stops calling SetEngine, so silence the loops here
        engineOn = false;
        skidAmount = 0f;
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;

        // music crossfade: fade out, swap clip, fade in
        if (musicSwitching)
        {
            musicFade = Mathf.MoveTowards(musicFade, 0f, dt / Mathf.Max(0.01f, musicFadeTime));
            if (musicFade <= 0f || !musicSource.isPlaying)
            {
                musicSource.Stop();
                musicSource.clip = pendingMusic;
                if (pendingMusic != null) musicSource.Play();
                musicSwitching = false;
            }
        }
        else if (musicSource.clip != null)
            musicFade = Mathf.MoveTowards(musicFade, 1f, dt / Mathf.Max(0.01f, musicFadeTime));
        musicSource.volume = musicVolume * cachedMusicVol * musicFade;

        // engine: pitch follows speed, fades out when the car stops running (game over, menus)
        UpdateLoop(engineSource, engineOn ? engineVolume : 0f, dt);
        engineSource.pitch = Mathf.Lerp(enginePitch.x, enginePitch.y, Mathf.Clamp01(engineSpeed / engineTopSpeed));

        UpdateLoop(skidSource, skidVolume * skidAmount, dt);
    }

    void UpdateLoop(AudioSource s, float targetVolume, float dt)
    {
        if (s.clip == null) return;
        s.volume = Mathf.MoveTowards(s.volume, targetVolume * cachedSfxVol, loopFadeSpeed * dt);
        if (s.volume > 0.001f) { if (!s.isPlaying) s.Play(); }
        else if (s.isPlaying) s.Stop();
    }

#if UNITY_EDITOR
    // Keep exactly one row per Sfx value, in enum order, so new sounds show up in the Inspector automatically.
    void OnValidate()
    {
        var ids = (Sfx[])Enum.GetValues(typeof(Sfx));
        if (sfx != null && sfx.Length == ids.Length)
        {
            bool aligned = true;
            for (int i = 0; i < ids.Length; i++) if (sfx[i] == null || sfx[i].id != ids[i]) { aligned = false; break; }
            if (aligned) { for (int i = 0; i < ids.Length; i++) sfx[i].name = ids[i].ToString(); return; }
        }

        var rebuilt = new SfxEntry[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            SfxEntry existing = null;
            if (sfx != null) foreach (var e in sfx) if (e != null && e.id == ids[i]) { existing = e; break; }
            rebuilt[i] = existing ?? new SfxEntry
            {
                id = ids[i],
                playWhilePaused = ids[i] == Sfx.UiClick,
                minInterval = ids[i] switch // starting values for sounds that can fire in bursts
                {
                    Sfx.ZombieNotice => 1.5f, // many zombies start chasing close together
                    Sfx.ZombieClaw   => 0.8f, // damage ticks several times a second
                    Sfx.CarScrape    => 0.3f,
                    _                => 0.05f,
                },
            };
            rebuilt[i].name = ids[i].ToString();
        }
        sfx = rebuilt;
    }
#endif
}
