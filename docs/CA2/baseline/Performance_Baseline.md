## during spawning
### frame time average
19.32ms at regular spawn intervals. Frame Rate toggle will likely be a important inclusion for my device's spec

### 99th percentile
Consistently around 33.66ms

## GC Alloc per frame,
was measured around 2.3KB per spawn.

**Changed:** Debug.Log + string declarations on each spawn was the cause, and commenting out the Debug.Log line fixed it. 

**Now:** Consistent 0B GC.Alloc for GameManager (The pool manager)

