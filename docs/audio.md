# Audio: sounds to find

Every sound below is already called from code through `AudioManager`. Until a clip is assigned, that call is silent. Drag clips into the matching row on the AudioManager in the Boot scene (several clips in one row = random variation).

## Sound effects

| Sfx row            | Plays when                                     | What to look for                                                  | Have it? |
|--------------------|------------------------------------------------|-------------------------------------------------------------------|----------|
| `UiClick`          | Any button with `UI_ButtonSound`               | Short soft click/tap                                              | y        |
| `UpgradeBought`    | Upgrade succeeds (`Wallet.TryUpgrade`)         | Ratchet/wrench + small chime, or cash register                    | y        |
| `CoinEarned`       | Coins earned in play (`GameManager.EarnCoins`) | Short coin clink                                                  | y        |
| `ZombieNotice`     | A zombie starts chasing                        | Groan/snarl. 3-4 variations (max 1 per 1.5s)                      |          |
| `ZombieLeap`       | A zombie jumps at the car                      | Short grunt/shriek                                                |          |
| `ZombieLand`       | A zombie lands on a window                     | Heavy thump on metal/glass                                        |          |
| `ZombieClaw`       | Damage while hanging on                        | Scratching/banging on a car door. 2-3 variations (max 1 per 0.8s) |          |
| `ZombieKnockedOff` | Shaken or scraped off                          | Yelp + body hitting tarmac                                        |          |
| `ZombieRunOver`    | Head-on hit with the front                     | Wet thud/splat on a bumper                                        |          |
| `CarScrape`        | Car body scrapes an obstacle                   | Short metal scrape (max 1 per 0.3s)                               | y        |
| `SlowArea`         | Driving over a slow-area hazard                | Gravel crunch or oil squelch                                      | y        |
| `Crash`            | Front crash, flip, or leaving the road         | Big car impact, metal and glass                                   |          |
| `Overrun`          | Health reaches 0                               | Engine sputtering out/stalling, ideally with a zombie roar        | y        |
| `RepairPickup`     | Collecting a repair box                        | Ratchet/wrench + positive chime                                   | y        |
| `GameOverSting`    | Results panel appears                          | 1-2s low, ominous sting                                           |          |

## Loops

| Field | Plays | What to look for | Have it? |
|---|---|---|---|
| Engine Loop | While playing, pitch rises with speed | Seamless engine idle/drive loop | Yes: `PROMETEO - Car Controller/Sounds/CarEngine.wav` |
| Skid Loop | Volume follows how hard the car is turning | Seamless tyre screech loop | Yes: `PROMETEO - Car Controller/Sounds/TireScreech.wav` |

## Music (per scene, by scene name)

| Scene | Mood | Have it? |
|---|---|---|
| `MainMenu` | Moody, slower; loops cleanly | |
| `Game` | Driving, tense, mid/high tempo; loops cleanly | |

Optional later: an ambient wind/road loop, and a low-health warning (heartbeat/beeping) under 25% health.

## Import settings (Android)

- **Short SFX:** Force To Mono on, Load Type **Decompress On Load**, Compression **Vorbis** quality ~70 (or ADPCM for very short, frequent ones like clicks and claws).
- **Loops (engine, skid):** Force To Mono on, **Decompress On Load**, Vorbis. Check the loop point is seamless.
- **Music:** Load Type **Streaming**, Vorbis quality ~50-60. Stereo is fine.
- Sample rate: **Override Sample Rate** to 22050 Hz for SFX keeps memory down with no audible loss on phone speakers.

Check licences and add sources to [credits.md](credits.md).
