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

Conclusion: more consistent time ms and less spikes on both CPU and GPU at low res, GPU bound.