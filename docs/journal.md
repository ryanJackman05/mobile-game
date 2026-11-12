14-09-2026

I had struggled with getting Unity 6 to install
I was stuck trying to get the build to run on my phone, and had to enable ARMv7 build support.
The game eventually succesfully installed and can now run on its own on my phone.

Selected Idea:
# 2 - Endless Runner
### Core Verb (?) - Drive

## Including:
- Procedural Biomes
- chunk generator 
- difficulty curves
- pooling. 

### Vertical-slice by W6: 1 biome, 10+ chunks, mission system, Android device build.

The main cut list of this project will be (in increasing order of importance):
- difficulty curves

16-09-2026

 CPU main thread ms: 6ms
 SetPass calls: 0 (?)
 GC allocated in frame: 4, 36B

21-09-2026
Checklist
| Test | Expected | |
|------|----------|-|
| Press Home, wait 10 s, return | Paused, panel visible, audio silent, progress saved | |
| Pull the notification shade down and up | Paused | |
| Neighbour calls you, you hang up | Paused, game resumes only on Resume | |
| Screen off with the power button, back on | Paused | |
| Force stop from Settings, relaunch | Progress restored from the save | |

25-09-26

did a little bit more. getting the car moving and trying to test on mobile was taking too long, so once I knew the touch detection was working as expected, I attached a KeyboardInput script and a PLayerInput Component to allow testing the game on my PC with rudimentary controls. This will speed up development a lot. Haptics are ready to be wired in later once things are added.

commented out main game loop methods, may need further additions but looks good for now

28-05

created car test model

created ramp, rudimentary jumps work

game props will use new MoveWithWorld component to "scroll" to emulate the car driving.

29-09

introduced & implemented basic object Pool

improved the car handling, this is likely how it wil stay for the final project

added terrain

30-09
 worst-frame main-thread ms = 38ms
 the tallest marker name = PostLateUpdate.FinishFramerendering
 top three hierarchy entries = PostLateUpdate.FinishFramerendering, PhysicsFixedUpdate, PresentAfterDraw
  GC.Collect no

03-10 
worked a lot today on the CA1 spec requirements.
I used a privacy policy generator website to make a Privacy Policy and Data Safety statemet for the game. I was dissatisfied with the unclear wording, so changed it to a ChatGPT-generated version.

04-10
actually continued the labs today. Did not realise the labs contained so much work on the CA1 files.

For the Part D: RenderScaleProbe test, here are the values
Before Toggle
Main Thread CPU time: between 17 - 33 ms
Gfx.WaitForPresentOnGfxThread: wild jumping between 8 - 25 ms

After toggle
Main Thread CPU time: early hovered around 16.7, before spikes started appearing again, less frequently than before.
Gfx.WaitForPresentOnGfxThread: average of 8-10 ms

Conclusion: more consistent time ms and less spikes on both CPU and GPU at low res, but minimum frame time did not drop very far. First conclusion is GPU-bound, but it may also point to not enough happening in game as of this version (no textures, simple models, etc)

REST (no menu yet)
2026/10/04 18:02:38.984 25221 25241 Info Unity [Baseline] avg 18.85 ms  p99 33.69 ms
GAMEPLAY (no high intensity case for this build, as the SpawnMany button is hidden)
2026/10/04 18:04:19.375 25221 25241 Info Unity [Baseline] avg 18.93 ms  p99 33.63 ms

5-10
Optimisations noted to focus on for Lab 5A:
- One manager ticks a List<ITickable>; hundreds of Update methods cost call overhead​
- Disable components that have nothing to do this frame​
- Never GetComponent, Find or Camera.main in a loop; cache in Awake​
- Use FixedUpdate only for physics; keep it light​

- Awaitable: Implement basic timers for reference, so they can be used instead of Coroutines if needed later.
    (Ask Pino if suitable for project)

- Ensure that Prewarm to the worst wave in Awake, behind the loading screen​
- add comment: Reset state on SPAWN: health, velocity, trails, timers​
- Cap the pool and log if you exceed it (compare to lab implementation)

Potential issues/checks:
  - Stale references: a released enemy is still targeted by another script​
  - Particle systems and trail renderers need Clear() on release​ (unlikely for this project)
  - A Rigidbody keeps its velocity; be sure to zero it on spawn​

***No coroutines in project***

1-11
Finally returned to working on game by continuing lab 5

3-11
added models for lab 5A and credited sources

Finished Lab 5A:

No coroutines found, as I had already implemented the Pool and spawn intervals with a rudimentary timer.

GC Alloc per-spawn: 2.3KB -> 0B

Surprising was that Debug.Log calls created such large memory allocation. Maybe using just a print() call would be smaller.

4-11
Finished Lab 5B
Need to revist part C & E

5-11
Finished Lab 6A

Skipped the last step to make up for lost time. I've decided that gameplay balancing and pacing can be handled at a later date.

I have added the Part E instructions to the Gamemanager as a TODO

9-11

2026/11/09 00:02:31.279 3417 3477 Info Unity [telemetry] 2.47 51e6c34c session_start app_version=0.3.1 device_model=samsung SM-A137F android=Android OS 14 / API-34 (UP1A.231005.007/A137FXXSCEZB1)

[telemetry] 1.77 7fe1094d level_start level_id=Game attempt=2 coins=0
UnityEngine.Debug:Log (object)
Telemetry:Log (string,System.ValueTuple`2<string, object>[]) (at Assets/Scripts/Telemetry/Telemetry.cs:18)
GameManager:NewGame () (at Assets/Scripts/GameManager.cs:96)
GameManager:Awake () (at Assets/Scripts/GameManager.cs:47)

[telemetry] 8.93 2a82a623 level_gameOver level_id=Game time_s=5.899941 coins_earned=0 score=60.0206277668476
UnityEngine.Debug:Log (object)
Telemetry:Log (string,System.ValueTuple`2<string, object>[]) (at Assets/Scripts/Telemetry/Telemetry.cs:18)
GameManager:GameOver () (at Assets/Scripts/GameManager.cs:127)
PlayerCarController:Crash (bool) (at Assets/Scripts/Player/PlayerCarController.cs:88)
UnityEngine.Component:SendMessageUpwards (string,object)
CrashDetector:OnTriggerEnter (UnityEngine.Collider) (at Assets/Scripts/Player/CrashDetector.cs:27)


[telemetry] 51.60 af133ca9 upgrade_purchased upgrade_id=speed level=4 cost=337 coins_left=388
UnityEngine.Debug:Log (object)
Telemetry:Log (string,System.ValueTuple`2<string, object>[]) (at Assets/Scripts/Telemetry/Telemetry.cs:18)
Wallet:TryUpgrade () (at Assets/Scripts/Economy/Wallet.cs:22)
UI_WalletHUD:Upgrade () (at Assets/Scripts/UI/UI_WalletHUD.cs:20)
UnityEngine.EventSystems.EventSystem:Update () (at ./Library/PackageCache/com.unity.ugui@23caec89ae27/Runtime/UGUI/EventSystem/EventSystem.cs:527)

11-11
Lab 6B Part D: Save and resume proof:
1. Background and resume - Success
2. Force-stop and relaunch - Success
3. Low-memory kill while backgrounded - Success
4. Log pull - Success

***Reflection due on Due Sun 1 Nov 2026, 23:59 (missed)***

12-11
LAb 7A Part D:
Perfmode flip working
2026/11/12 13:36:22.747 5232 5267 Info Unity perf_mode=Balanced, high=False
2026/11/12 13:39:21.081 5232 5267 Info Unity perf_mode=HighFPS, high=True

Frequent alternation of bottleneck cause, and bottleneck occuring often
2026/11/12 13:14:25.260 30320 30409 Info Unity [AP] bottleneck=Unknown
2026/11/12 13:14:25.462 30320 30409 Info Unity [AP] bottleneck=GPU
2026/11/12 13:14:26.449 30320 30409 Info Unity [AP] bottleneck=Unknown
2026/11/12 13:14:29.243 30320 30409 Info Unity [AP] bottleneck=GPU
2026/11/12 13:14:30.698 30320 30409 Info Unity [AP] bottleneck=TargetFrameRate
2026/11/12 13:14:41.923 30320 30409 Info Unity [AP] bottleneck=GPU
2026/11/12 13:14:43.759 30320 30409 Info Unity [AP] bottleneck=Unknown
2026/11/12 13:14:44.427 30320 30409 Info Unity [AP] bottleneck=GPU
2026/11/12 13:14:45.617 30320 30409 Info Unity [AP] bottleneck=TargetFrameRate

could not provide multiple screenshots due to graphical error when in HighFPS, and 60Hz limit on my phone's display.
Tried both modes in one profile & Screenshotted that. Transition point labelled.

13-11

14-11