# Known Issues

Remaining issues and missing features at the end of feature work (15-11-2026). Cut features (missions, unique zombie variants, speed power-up, menu stack) aren't listed. See [docs/priorities.md](docs/priorities.md) for the history.

## QA

- Lab 8A QA tests: scenarios 1 and 2 throw strange console log exceptions. See [test-plan.md](docs/CA3/qa/test-plan.md).
- Async Pack 3 (peer APK and survey, closes Sun 29 Nov 2026): [device-matrix.md](docs/CA3/qa/device-matrix.md) and [form-link.md](docs/CA3/qa/survey/form-link.md) are still placeholders.

## Build

- Adaptive Performance logging is still on in all three provider settings (`Assets/Adaptive Performance/Settings/`). Turn it off for the CA2 build.
- Debug logs still print in play: `[AP] ...` (AdaptiveBootstrap), `[perf] scene ... loaded` (SceneLoader), `Show: ...` (UI_ResultsMenu), `Game Over: ...` (GameManager).
- ThermalOverlay (ms / thermal debug text) is visible in the Game scene.

## Gameplay

- Death doesn't feel right. The results panel waits 1.2s (`UI_ResultsMenu.showDelay`) without enough feedback during that time (no camera shake, cause flash or similar). Either add feedback or set the delay to 0.
- No chunk-based spawning. Obstacles, zombies and poles spawn at random positions on distance timers. The MDA's vertical slice asked for 10+ chunks.
- Repair pickups only appear on 3 obstacle types (gap1, ramp, ramp & container). There are no open-road pickups.
- Jumps are not implemented. Ramps exist as obstacles, but there's no jump handling (`PlayerCarController.Jump` / `StopJump` are empty).
- No difficulty curve beyond a constant speed ramp (+3 per minute, no cap). Spawn rates and road width don't change with difficulty.
- Fuel meter (journal) not decided: neither built nor cut.
- Values for player control, zombies and difficulty are not fine-tuned (moved to CA3).
- Lab 6A Part E economy tuning (first upgrade inside 2 minutes) is not done. Steps are in the `// TODO s` block at the bottom of `GameManager.cs`.

## Audio

- Missing SFX: ZombieNotice, ZombieLeap, ZombieLand, ZombieClaw, ZombieKnockedOff, ZombieRunOver. These events are silent.
- No music for MainMenu or Game.
- No volume settings in any menu (`UI_VolumeSlider` exists but isn't placed).

## Art and UI

- Lab 9A Part C materials and UI pass not revisited for CA2.
- Accessibility pass incomplete ([accessibility-pass.md](docs/CA1/accessibility-pass.md) says "STILL TODO").
- `TextScale` should be on every TMP text, but whether it is hasn't been checked (`TextScale.cs` has a bare TODO).
- Performance mode toggle gives no feedback when changed (`UI_PerformanceModeToggle.cs`).
- Health bar label text is written as `"${health}/${max}"`, which shows literally. It's harmless right now because no label is assigned on `UI_HealthBar`.

## Code

- MDA one-pager still describes low speed as a lose condition. In the game, health is the fail condition and speed is a penalty only.
- `GameManager.Pause()` / `GameState.Paused` are never used (pausing is handled by LifecycleGuard).
- Pools aren't prewarmed while the loading screen holds at 90% (`SceneLoader.cs` TODO).
- `TapSwipeInput.cs`: open question about removing swipe detection at the end of a hold (Lab 2A).
- `ResetCar()` teleports through the transform while the Rigidbody is interpolated, which may show a one-frame smear on restart (not confirmed).
