# SPEED-HEIST — Step 02 / Local Unity setup

## Current status
Unity project source committed. Unity Editor compilation and Android APK **not** verified here (no Unity Editor/Android SDK installed in execution environment). Unity-generated starter scene, renderer assets, materials and `.meta` must be generated using the Editor and committed after validation.

## Toolchain
- Unity Hub + Unity **6000.3.27f1 LTS**
- Android Build Support, Android SDK/NDK Tools, OpenJDK via Unity Hub
- Unity 6.3 URP 17.3.0 and Input System 1.20.1 from `Game/Packages/manifest.json`
- Git, VS Code + C# support

## Linux Mint commands
```bash
git clone https://github.com/rekixo-lab/SPEED-HEIST.git
cd SPEED-HEIST
git status
```

1. Install the pinned Editor in Unity Hub, including Android Build Support. Linux Mint is not one of Unity's officially tested Editor distributions; verify it works.
2. Unity Hub > Add project from disk > select **SPEED-HEIST/Game**, not repository root.
3. Wait for Package Manager and C# compilation. Clean imports attempt first-run scene generation. Otherwise use **Tools > SPEED-HEIST > Generate Starter Scene**.
4. Open `Assets/_SpeedHeist/Scenes/PrototypeRace.unity`. Hit Play. A/D or arrows to steer, Space to jump; on phone drag the left half to steer and tap right half to jump.
5. Set Project Settings > Player > Active Input Handling to **Input System Package (New)** if necessary. Check URP asset in Project Settings > Graphics.
6. Build using **Tools > SPEED-HEIST > Build Android Test APK**. Local output: `Game/Builds/SpeedHeist-Debug.apk`.
7. On connected phone: `adb devices`, then `adb install -r Game/Builds/SpeedHeist-Debug.apk`.
8. Review Unity-generated scene/URP/materials and commit their assets and meta files. Do not commit `Library`, `Temp`, `Builds`.

## Current prototype scope
One capsule runner, graybox track, steering, jump, follow camera, finish trigger, temporary in-game HUD. No Speed Ball, networking, bots or final artwork yet.

## Step 02 exit criteria (pending)
- Project opens with no compiler errors
- Unity-generated scene and URP are valid
- Runner/controls work in Editor
- Android debug APK builds and runs on real phone
- Newly generated assets are checked into source control
