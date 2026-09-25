# Title & one-line pitch
## Ride or Die
A highway endless runner where you manuever your car to avoid or fight off endless number of zombies, all while driving to salvation.

# Aesthetics (target player feelings/experiences)
The player should feel thrilled and excited. There will be jumps, tight spaces, occasional missions, and many zombies that will try to jump onto your car.

Location types? At least one. - Desert, city, tunnel, suburban

When the player loses, what happens - stall & menu popup? zombie horde chasing car animation?

# Core mechanics (3–5 verbs/systems)
Driving/Steering (core action)

killing zombies (collision or scraping),

procedural zombies & obstacles

power ups / repair pickups

player speed (Difficulty & lose condition)
 
 
# Dynamics (how play unfolds)
The player will begin each run at a slowish speed, and speed up over time. They will control their car and try to avoid obstacles such as debris, abandoned vehicles, and fires. 

Zombies will be trying to jump onto the player's car, and will damage the car until the player loses. They can be removed by driving into obstacles (or jumps?)

Pickups (such as one-use power ups, or repair boxes) will be placed in hard-to-reach spots (determined by the "chunks" during random generation) such as at the top of a ramp, or between a tight space.

Scraping too many obstacles or hitting too many zombies consequetively will bring down the speed, and the player will lose if the speed goes below a certain threshold. The player will also lose if they crash into a hazard, or flip the car on a jump.

Driving out of bounds? Invisible wall easiest

# Progression & content (levels/biomes/sessions; run length)
level environments will change as time goes on, correlating with the difficulty level. There should be a theoretical "limit" to the difficulty, but a run can still go on forever.

# Platform features (haptics, safe-area handling; store/testing tracks = awareness only)

# Performance budget (target frame-time, memory, load time)
30FPS minimum (33.3ms)

Memory (???GB)

Load time: < 10 secs

# Monetisation (if any) & ethics notes
None

# Risks & cuts list 
### top of list is first to go, and will be marked with X when omitted
- missions
- difficulty curves
- Jumps
- unique zombie variants
- speed power up
- repair power up

`Vertical-slice by W6: 1 biome, 10+ chunks, mission system, Android device build.`