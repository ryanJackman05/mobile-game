# Priorities (15-11-2026)

Built by comparing the [MDA one-pager](CA1/mda-onepager.md) with the TODOs in `Assets/Scripts` and the [journal](journal.md).

## Open decision: lose condition

The MDA, the code and the journal each describe a different way to lose:

| Source | How you lose |
|---|---|
| MDA | Speed drops below a threshold after too many scrapes or zombie hits. Also crashing or flipping. |
| Code (now) | Health reaches 0, drained by zombies on the windows. Also crashing (Front/Top triggers) and going out of bounds. |
| Journal | Fuel meter: lose if fuel isn't collected often enough. |

Recommendation: keep **health**. It's already wired to zombies and to the upgrade (`maxHealth` grows with level), and the MDA's repair pickup fits it. Drop fuel. Speed could become a penalty (scrapes slow you down) rather than a fail state. Once decided, update the MDA so the doc and the game agree.

Smaller conflicts with the MDA:
- Hitting a zombie head-on slows you down in the MDA, but the code gives +25 coins.
- The MDA knocks zombies off by driving into obstacles. The code currently uses hard steering. Scraping the car's side against an obstacle could be added as a second way.

## P0: has a deadline or blocks a build

- [ ] **Async Pack 3 survey (closes Sun 29 Nov).** Fill in [device-matrix.md](CA3/qa/device-matrix.md) and [form-link.md](CA3/qa/survey/form-link.md), which the CA3 QA evidence pack depends on.
- [ ] **Before the CA2 build:** turn off Adaptive Performance logging (`GameManager.cs:209`).

## P1: the core loop works and is readable

- [x] **Zombies finished:** animations, remove the placeholder capsule, tuning, knock-off by scraping obstacles.
- [ ] **Health bar on screen.** Zombies drain health but nothing shows it.
- [ ] **Health as the fail condition, plus the Upgrade button on the main menu** (journal). The health part is mostly done.
- [ ] **Game over presentation and cause:** add the cause to telemetry (`GameManager.cs:145`) and a unique death (`PlayerCarController.cs`).
- [ ] **Haptics** on zombie hits and pickups (`Haptics.cs:13`). Landing on a window is done.

## P2: content from the MDA's mechanics

- [ ] **Repair pickup**, plus `itemPool` (`GameManager.cs:89`).
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