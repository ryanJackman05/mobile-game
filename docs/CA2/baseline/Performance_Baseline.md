## during spawning
### frame time average
19.32ms at regular spawn intervals. Frame Rate toggle will likely be a important inclusion for my device's spec

### 99th percentile
Consistently around 33.66ms

## GC Alloc per frame,
was measured around 2.3KB per spawn.

**Changed:** Debug.Log + string declarations on each spawn was the cause, and commenting out the Debug.Log line fixed it. 

**Now:** Consistent 0B GC.Alloc for GameManager (The pool manager)

## 5B
### Cold Starts:
1066, 931, 997
Target: 2-3 seconds max

### Time to Interactive:
2026/11/04 22:18:27.241 6897 6935 Info Unity [perf] interactive 2.83s

### Time to Menu:
2026/11/04 22:41:53.959 10031 10090 Info Unity [perf] scene MainMenu loaded in 737 ms

### APK Size:
35.0 MB (ARM32)

There will be time inconsistencies due to me failing to perform the After captures immediately after the soak

| Field | Value                                                                                                          |
|-------|----------------------------------------------------------------------------------------------------------------|
| Build ID and mode | v0.4.0 (xxxxxxxx), Balanced                                                                                    |
| Start temperature (C) | temperature: 281, 14:05                                                                                        |
| End temperature (C) and 10-minute delta | temperature: 310, 14:16 - 29 delta                                                                             |
| Overlay level at minute 0 / 5 / 10 | NoWarning,0.65,0 / NoWarning,0.82,0 / NoWarning,0.82,0 (same)                                                  |
| First `ThrottlingImminent` (minute, logcat time) | Not observed in 10 min, 0.82 max. See below                                                                    |
| `Throttling` observed (yes/no, minute) |                                                                                                                |
| Frame time before: mean / 99th (ms) | mean = 19.78ms    p99 = 37.05ms (taken from Analyser Max value, as lab suggested a P99 value that didn't exist |
| Frame time after: mean / 99th (ms) | mean = 20.23ms    p99 = 45.34 ms                                                                               |
| Conditions | unplugged, brightness 50%, case on (whoops), ambient about 15 C                                                |

First THrottling imminent (much later):
<i>AndroidPlayer "Samsung_SM A137F@ADB:R58T9052TRP:0"</i> [Adaptive Performance] SkinTemp=0.85 PerformanceMode=Standard
Performance level CPU=-1/-1 GPU=-1/-1 thermal warn=ThrottlingImminent(1) thermal level=0.85 mode=Automatic
Average GPU frametime = 19.30 ms (Current = 19.95 ms)
Average CPU frametime = 9.32 ms (Current = 9.88 ms)
Average frametime = 20.91 ms (Current = 16.74 ms)
Bottleneck GPU, ThermalTrend 0.186
CPU Boost Mode False, GPU Boost Mode False
Cluster Info = Big Cores: 0 Medium Cores: 0 Little Cores: 0
FPS = 47.81
Performance Mode = Standard




