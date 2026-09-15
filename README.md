# mobile-game [Title Undecided]
Project for Labs and CA development in the Mobile Game Development module

## Option Number 
2 [Endless Runner]




## Build steps
```bash
adb devices
# List of devices attached
# R58M12ABCDE   device

adb install -r Builds/MyGame-dev.apk
# Performing Streamed Install
# Success

adb shell monkey -p com.ryanJackman.coolMobileGame 1
adb logcat -s Unity
# [Boot] SM-S911B | Android OS 15 / API-35 | Vulkan | 1080x2340 @ 425 dpi
```

confirmed boot debug output: `[Boot] samsung SM-A137F | Android OS 14 / API-34 (UP1A.231005.007/A137FXXSCEZB1) | Vulkan | 1080x2408 @ 450 dpi`