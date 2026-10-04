# mobile-game (Ride or Die)
Project for Labs and CA development in the Mobile Game Development module

## Option Number 
2 [Endless Runner]




## Build steps
1. Clone repo with `git clone https://github.com/ryanJackman05/mobile-game.git`
2. open as unity project in Unity 6.6 (6000.6.0f1 if possible)
2. Switch Build Platform: This can be achieved with a new Build Profile. Ideal settings below:
- Configuration > Scripting Backend: IL2CPP
- Android Application Configuration > Target Architectures: ARM64 (v7 optional, beware of larger APK size)
- Set package name & versionCode. *Remember both.*
- File > Build Profiles: Select build profile, hit "Build". Select a suitable install location and APK name (eg. MyGame.apk, *remember this too*)

Now we install using the Android ADB commands (assuming ADB is installed and USB Debugging Enabled.)
```bash
adb devices
# Returns List of devices attached
# R58M12ABCDE   device

adb install -r Builds/MyGame.apk # << APK name from before
# Performing Streamed Install
# Success

adb shell monkey -p com.ryanJackman.coolMobileGame 1 # << Package name from before.
# You can optionally launch the game by searching up the Product Name field inside All Apps.
adb logcat -s Unity
# [Boot] SM-S911B | Android OS 15 / API-35 | Vulkan | 1080x2340 @ 425 dpi
```

## Uninstall steps
```bash
adb uninstall com.ryanJackman.coolMobileGame

adb install -r Builds/MyGame-dev.apk > docs/CA1/install-proof.txt

adb shell monkey -p com.ryanJackman.coolMobileGame 1
```



### Device Targets
confirmed boot debug output: `[Boot] samsung SM-A137F | Android OS 14 / API-34 (UP1A.231005.007/A137FXXSCEZB1) | Vulkan | 1080x2408 @ 450 dpi`
API Level - Android 8.0 (API Level 26) [Default by unity]

Tested Devices:
- Samsung A13
- Samsung A12

___

### Signing
"C:\Users\ryanj\My Stuff\Unity\Keystores\mygame-release.keystore"

Alias: "coolmobilegame"
Valid from: Wed Sep 16 10:03:00 IST 2026 
        until: Thu Sep 03 10:03:00 IST 2076
Certificate fingerprints:
         SHA256: 64:03:0A:C1:8D:48:CB:ED:FD:72:BB:AE:84:22:11:7A:78:56:25:11:90:3B:8E:0D:3F:CD:36:4E:B6:76:55:C9


--- 

### AI Assistance
AI was not used to generate any code at the current build version of the game 0.2.0
AI was used to generate a template Data Privacy Statement for CA1
