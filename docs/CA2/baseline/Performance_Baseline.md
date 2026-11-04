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