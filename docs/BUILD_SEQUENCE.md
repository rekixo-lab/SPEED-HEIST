# SPEED-HEIST — Implementation sequence and gates

**Rule:** Proceed in the following order. Exit criteria are more important than feature count. Unity project and playable APK have **not yet been created**.

| Step | Work | Main software | Gate to proceed |
|---|---|---|---|
| 01 | Confirm gameplay rules, anti-exploits, race win/respawn rules, scope | Markdown/Figma | One reviewed game design |
| 02 | Initialize Unity URP project at `Game/`, Input System, Android support, Git | Unity / VS Code / Git | Empty Android build launches on phone |
| 03 | Character movement, follow camera, jump, collisions, touch input | Unity / C# | Responsive touch running on phone |
| 04 | Speed reserve, spawn/throw/pickup/steal orb, conservation tests | Unity / C# | Energy cannot duplicate or underflow |
| 05 | One graybox track, shortcuts, obstacles, checkpoints, time and finish | Unity / C# | Full solo race finishes reliably |
| 06 | Shared network simulation contracts and host authoritative events | Unity Netcode / Transport | Two connected players share race state |
| 07 | Same-Wi-Fi / hotspot direct LAN connect; no cloud dependency | Unity Transport | 2–4 real phones play without internet |
| 08 | Internet private rooms, compatible session/connectivity services | Unity / Backend | 2–4 online clients complete a race |
| 09 | Bot runners, four-player balancing, disconnect/timeout states | Unity / C# | Mixed human/bot race is coherent |
| 10 | Produce one optimized final environment and track art | Blender / Unity | Readable, performant complete map |
| 11 | HUD, match flow and error states; accessible safe-area layouts | Figma / Unity UI | Screen and input tests on devices |
| 12 | Character meshes, rigs, animation states, customization hooks | Blender / Unity | Animations match controller |
| 13 | Sound, trails, particles, haptics, camera feedback, polish | Unity VFX / Audacity | Effects clear without obscuring hazards |
| 14 | Minimal progression, non-pay-to-win rewards; optional modes later | Unity / Backend | Rules remain fair, saves are robust |
| 15 | Performance, regressions, network fault, security and accessibility | Unity Profiler / Android tools / CI | Release checklist passes on device matrix |
| 16 | Signed Android AAB, closed testing and publishing, iOS later | Unity / Play Console / Xcode | Store and release requirements met |

## Folder layout planned for Step 02

```text
SPEED-HEIST/
  README.md
  .gitignore
  docs/
    GAME_DESIGN.md
    BUILD_SEQUENCE.md
  Game/                  # Unity project to be generated, NOT present yet
    Assets/
      _SpeedHeist/
        Core/
        Gameplay/
        Networking/
        AI/
        UI/
        Art/
        Audio/
        Tests/
    Packages/
    ProjectSettings/
```

## Engineering guardrails
- Android first and test on a physical device frequently.
- Single source of truth for the boost reserve, orb pickup and match results.
- Maintain `.meta` files; track Unity `Assets`, `Packages`, `ProjectSettings`; never commit Unity `Library`.
- Add Git LFS for large binary source art/assets **before** importing any large files.
- Validate Unity package compatibility and lock the editor version before project initialization.
- Keep online infrastructure separate from direct-IP LAN so offline play truly works offline.
- Add automated unit tests for energy invariants and integration tests for race/session lifecycle; supplement with real-device network tests.
- Protect baseline performance and memory; avoid developing only on a high-end desktop.
- Never treat concept mockups as integrated game UI or a build as "production ready" without device validation.

## First actual coding task
Create Unity URP Android project at `Game/`, implement touch-ready runner controller + simple camera with one graybox runway, and produce a test APK. Before that, installation and editor compatibility must be checked on the development machine.
