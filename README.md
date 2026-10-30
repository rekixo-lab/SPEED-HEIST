# SPEED-HEIST

A planned stylized 3D multiplayer running game for Android first, with iOS support later.

> **Status:** Pre-production / game design. This repository does **not** yet contain a playable Unity project.

## Game identity
Run, store your temporary speed energy in a throwable orb, recover it or risk another runner stealing it, and cross the finish line first. Speed transfers are the distinguishing mechanic—not generic lane switching or purchased speed advantages.

## Target first playable
- 1 graybox race track, 2–4 runners, 90–120 second target race
- Touch steering, jump, follow camera, checkpoints, finish and race results
- Speed-orb store / throw / pickup / steal (authoritative, no energy duplication)
- Solo bots first; later same-Wi-Fi / hotspot multiplayer; then online private rooms
- Android first; iOS later

## Development documents
- [Game design and initial rules](docs/GAME_DESIGN.md)
- [Ordered implementation plan and quality gates](docs/BUILD_SEQUENCE.md)

## Proposed tooling
- Unity 6 LTS, C#, Unity Input System, Universal Render Pipeline (URP)
- Blender for 3D art; Figma for mobile UI; GitHub for source/versioning
- Netcode for GameObjects and Unity Transport for the small-session prototype; select compatible Unity multiplayer service packages when online is implemented

## Project layout (planned)
- `Game/` — Unity project root (will be added during setup)
- `docs/` — design, plans and decisions

## Next action
Install and verify the Unity Editor + Android Build Support on the development machine, initialize `Game/` as a URP project, commit `ProjectSettings`, `Packages`, `Assets` and their `.meta` files, and produce the first Android test build. Do not claim an APK or multiplayer is available until it is actually built and tested.

All numbers in the design document are starting playtest hypotheses, not finalized balancing.
