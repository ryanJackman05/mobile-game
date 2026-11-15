# Priorities (15-11-2026)

Built by comparing the [MDA one-pager](CA1/mda-onepager.md) with the TODOs in `Assets/Scripts` and the [journal](journal.md).

## Open decision: lose condition

The MDA, the code and the journal each describe a different way to lose:

| Source | How you lose |
|---|---|
| MDA | Speed drops below a threshold after too many scrapes or zombie hits. Also crashing or flipping. |
| Code (now) | Health reaches 0, drained by zombies on the windows. Also crashing (Front/Top triggers) and going out of bounds. |
| Journal | Fuel meter: lose if fuel isn't collected often enough. |

**Decided (15-11): health is the fail condition. Speed is a penalty only (scrapes, slow areas, head-on zombie hits).** Fuel not decided yet. Still to do: update the MDA to match.

Original recommendation: keep **health**. It's already wired to zombies and to the upgrade (`maxHealth` grows with level), and the MDA's repair pickup fits it. Drop fuel. Speed could become a penalty (scrapes slow you down) rather than a fail state. Once decided, update the MDA so the doc and the game agree.

Smaller conflicts with the MDA:
- Hitting a zombie head-on slows you down in the MDA, but the code gives +25 coins.
- The MDA knocks zombies off by driving into obstacles. The code currently uses hard steering. Scraping the car's side against an obstacle could be added as a second way.

## P0: has a deadline or blocks a build

- [ ] **Async Pack 3 survey (closes Sun 29 Nov).** Fill in [device-matrix.md](CA3/qa/device-matrix.md) and [form-link.md](CA3/qa/survey/form-link.md), which the CA3 QA evidence pack depends on.
- [ ] **Before the CA2 build:** turn off Adaptive Performance logging (`GameManager.cs:209`).

## P1: the core loop works and is readable

- [x] **Zombies finished:** animations, remove the placeholder capsule, tuning, knock-off by scraping obstacles.
- [x] **Health bar on screen.** Script done (`UI_HealthBar`); needs the UI object in the Game scene.
- [x] **Health as the fail condition, plus the Upgrade button on the main menu** (journal). Code done; move the button in the Editor.
- [x] **Variable speed:** slowdowns on scrapes, slow areas and head-on zombie hits, recovering to the base ramp.
- [x] **Game over presentation and cause:** cause in telemetry and on the results panel, panel delayed 1.2s, overrun death stalls instead of tumbling. Results labels need wiring in the Editor.
- [x] **Haptics:** zombie landing, hit and scraped off, car scrapes, crashes. Pickups move with the repair pickup (P2).
- [ ] **Death feels off.** The 1.2s results delay (`UI_ResultsMenu.showDelay`) only works if the death itself reads clearly: add feedback during it (e.g. crash sound, camera shake, a "CRASHED" / "OVERRUN" flash, slow-mo) or set the delay to 0.

## P2: content from the MDA's mechanics

- [x] **Repair pickup.** Repair Box children on obstacle prefabs, re-rolled each spawn by `ObstaclePickups` (15-11). Remove MoveWithWorld from the Repair Box prefab. Pickup haptics included.
- [ ] **Basic audio**, plus revisiting Lab 9A Part C for audio, materials and UI.
- [ ] **Chunk-based spawning** (vertical slice: 10+ chunks). Pickups in hard-to-reach spots depend on chunks. This also covers per-type obstacle pools (`GameManager.cs:37`).
- [ ] **Lab 8B parts C–E.**

## P3: cut list (MDA order, first to go at the top)

- [ ] Missions: recommend cutting now and marking them X in the MDA.
- [ ] Difficulty curves (speed already ramps up forever, which covers "gradual acceleration").
- [ ] Jumps
- [ ] Unique zombie variants
- [ ] Speed power-up

## Probably done already: confirm and remove

- [x] 30 FPS graphics mode (journal): `DeviceTier.cs` already caps Low tier at 30.
- [ ] Menu stack (`PauseMenu.cs:23`): marked as cuttable.

## TODOs with no detail: rewrite or delete

- [ ] `GameManager.cs:84`: `NewGame() //TODO`
- [ ] `GameManager.cs:122`: is the `Paused` state used?
- [ ] `TextScale.cs:6`
- [ ] `SceneLoader.cs:37`: pool prewarm
- [ ] `UI_PerformanceModeToggle.cs:25`
- [ ] `TapSwipeInput.cs:41`
- [ ] `CA1/accessibility-pass.md`: "STILL TODO"

## Moved to CA3

- [ ] Fine tuning of values for player control, zombies, difficulty, etc