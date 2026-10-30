# SPEED-HEIST — Game Design Document (initial proposal)

**Status:** Draft v0.1 · 2026-10-30  
**Scope:** Android-first multiplayer runner, offline solo + true local-network multiplayer + internet multiplayer, iOS later.

## 1. Player fantasy
A fast, colorful 3D race where the player can turn their *temporary speed reserve* into a physical orb, toss it across the track, retrieve it for a burst, or have it stolen by a rival. The orb is a tactical resource that changes racing decisions.

## 2. Design pillars
1. Understandable within 15 seconds; thumb-friendly.
2. Runs are short and replayable; avoid three fixed Subway-Surfers-style lanes.
3. Fair competition: skill and timing, not pay-to-win.
4. The same core movement/speed rules run in solo, LAN and online matches.
5. Good performance on midrange Android devices.

## 3. MVP match rules (playtest values)
- **Players:** 2–4. Solo can add AI bots.
- **Race:** one compact circuit with alternate paths, jumps and hazards; estimated 90–120 seconds for an ordinary finish.
- **Winner:** first runner to cross the actual finish collider after all checkpoints, as validated by the authoritative match host. Ties resolved by authoritative crossing timestamp, then deterministic fallback if timestamps are identical.
- **Timeout:** provisional 150 seconds; unfinished runners are DNF and placed by checkpoint progress and distance. Never let an endless match block results.
- **Starting fairness:** equal baseline movement, collision capsule and starting speed energy. Cosmetics never alter competitive stats.
- **Failure:** fall or hazard triggers a checkpoint respawn plus a fixed time penalty; no random instant elimination in Race mode.
- **Track:** free steering along designed surfaces; two or more readable route choices, not a mandatory three-lane runner.

## 4. Touch controls
- Left thumb: steer; continuous auto-forward movement in Race mode.
- Right thumb: jump; separate prominent action to store/throw speed orb (context-dependent with a short tutorial).
- Provide sensitivity and handedness settings later; ensure fingers do not cover race-critical content.

## 5. Speed-orb mechanic — authoritative conservation rule
**Important:** A player cannot manufacture extra energy by repeatedly throwing/catching an orb.

A proposed first playable model:
1. Each runner has normal base speed and a bounded **temporary boost reserve** (0–100 units). This is distinct from minimum running speed.
2. Energy is earned through designed on-track pickups/actions (to be balanced). At least 25 units are required to store an orb.
3. **Store:** subtract 25 units *immediately* from the player's boost reserve and spawn one orb containing 25 units. It never reduces movement below safe baseline speed.
4. **Throw:** launch the orb along an aimed, bounded ballistic arc. Physics bounce/landing is readable; the orb lasts a limited time before expiring.
5. **Pickup:** the first valid runner whose capsule overlaps the orb claims it. Their boost reserve increases by the orb's remaining units, capped at 100; excess is discarded. The orb despawns atomically.
6. **Steal:** a rival can pick up the same orb. There is no ownership lock after a small throw-release grace period, subject to playtest fairness.
7. **Use:** the runner consumes boost reserve for a temporary acceleration, capped by a fixed max speed. Collision/physics cannot multiply the stored units.
8. Host/server owns orb spawn, movement state, pickup, reserve adjustment and despawn. Clients send input and receive corrections.

**Initial targets, not final numbers:** baseline speed ~7 m/s, top boost ~11 m/s, orb 25 energy, 8-second lifetime, 0.2-second self-pickup lockout after release. Tune by playtests and network metrics, not guesswork.

**Why this matters:** Rivals can intercept an orb, but gaining one always corresponds to energy leaving somewhere else. The core mechanic must still be enjoyable without the perfect throw; do not punish players for learning.

## 6. MVP world
- One compact graybox "Sky City" course with ramps, safe route, risky shortcut, elevated bridge and checkpoints.
- Obstacles must have clear anticipation, collision feedback and recovery.
- Prototype with primitive geometry before Blender production art.
- Third-person follow camera with stable horizon and adjustable field of view.

## 7. Player modes and connection architecture
**Offline solo:** race with bots, no account/internet requirement.

**Offline local multiplayer:** 2–4 devices on same Wi-Fi or hotspot, one elected host device; direct local IP/port connection, with optional discovery later. Must not require cloud authentication, Lobby or Relay. Test Android hotspot client isolation and local firewall behavior.

**Online private multiplayer:** small session using appropriate Unity Lobby/Relay or equivalent service. Host-authoritative MVP, acknowledge limitations of a player-hosted competitive game.

**Ranked/public play (post-MVP):** evaluate dedicated authoritative servers, matchmaker, anti-cheat, reconnect and abuse controls before launch. Do not assume the casual host can prevent host-side cheating.

**Shared simulation contracts:** player input, race clock, checkpoint/finish, boost reserve, orb state, results, disconnection all have one owner-of-truth model. Unity physics is not deterministic across every device; do not depend on unmanaged lockstep.

## 8. Essential screens
Opening/loading; mode select; offline/LAN/online lobby; in-match HUD (position, elapsed time, reserve, jump and orb actions); pause; results; error/reconnect states. Final designs are made in Figma then implemented as Unity UI.

## 9. MVP explicitly excludes
Paid power boosts, seasonal live-service, eight-player matches, production cosmetics catalog, cross-platform account sync, public ranked matchmaker, chat, multiple maps, complicated weather and destruction, and iOS store publishing.

## 10. Acceptance tests before expanding
- Full race completes and results agree across four Android devices on local Wi-Fi.
- First and second pickup attempts for a single orb cannot duplicate energy.
- Players cannot claim or spend energy they do not own.
- Jump, turning, checkpoint and finish remain stable at various frame rates.
- Loss and reconnection behavior is explained and does not silently invent winners.
- Online 2–4 player room works under variable latency; test packet loss, timeout and host departure.
- Android build performance checked on real midrange phones, not just the editor.

## 11. Open design experiments
- Does storing energy feel like a meaningful decision, rather than just delaying boost?
- Should throws be aim-assisted on mobile?
- How far can a thrown orb go without causing unfair interception?
- Does auto-run versus manual acceleration produce better controls?
- What level of bumping, if any, is fun rather than frustrating?

**Decision rule:** Prove the movement and orb mechanic with a small graybox prototype before producing a full art set.
