# mobile-game (Ride or Die)
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

## Uninstall steps
```bash
adb uninstall com.ryanJackman.coolMobileGame

adb install -r Builds/MyGame-dev.apk > docs/CA1/install-proof.txt

adb shell monkey -p com.ryanJackman.coolMobileGame 1
```


confirmed boot debug output: `[Boot] samsung SM-A137F | Android OS 14 / API-34 (UP1A.231005.007/A137FXXSCEZB1) | Vulkan | 1080x2408 @ 450 dpi`
API Level - Android 8.0 (API Level 26) [Default by unity]

___

### Signing
"C:\Users\ryanj\My Stuff\Unity\Keystores\mygame-release.keystore"

Alias: "coolmobilegame"
Valid from: Wed Sep 16 10:03:00 IST 2026 
        until: Thu Sep 03 10:03:00 IST 2076
Certificate fingerprints:
         SHA256: 64:03:0A:C1:8D:48:CB:ED:FD:72:BB:AE:84:22:11:7A:78:56:25:11:90:3B:8E:0D:3F:CD:36:4E:B6:76:55:C9