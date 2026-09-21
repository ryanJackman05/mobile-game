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