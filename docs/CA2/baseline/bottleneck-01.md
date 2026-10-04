Before Toggle
Main Thread CPU time: between 17 - 33 ms
Gfx.WaitForPresentOnGfxThread: wild jumping between 8 - 25 ms

After toggle
Main Thread CPU time: early hovered around 16.7, before spikes started appearing again, less frequently than before.
Gfx.WaitForPresentOnGfxThread: average of 8-10 ms

---


What (one sentence): GPU occasionally spikes, likely due to the device being used (ARMv7 build, budget phone.)

Where (marker or pass name): Gfx.WaitForPresentOnGfxThread

Numbers: 
main-thread ms - between 17 - 33 ms
SetPass - 125 at start, stable 9
GC alloc - none
Frame Time (Gfx.WaitForPresentOnGfxThread) - 
full - 8-25ms
half - 8-10ms

Verdict: Previous verdict was GPU bound, but I can see now that when ignoring the spikes, the minimum frame barely decreased. Either CPU bound or gameplay is not yet intense enough. The main cause of longer frames seemed to be caused by a spike in Gfx.WaitForPresentOnGfxThread that occured roughly on every third frame

Fix to try: Continue implementing pooling and add actual models and textures before testing again.

Will update again later.

---

**The screenshots and data can be found in this folder. See below**
[capture data](w03-profile.data)
[Frame Debugger Screenshot](w03-gpu-pass.png)
[Analyser Bad Frame Screenshot](w03-bad-frame.png)