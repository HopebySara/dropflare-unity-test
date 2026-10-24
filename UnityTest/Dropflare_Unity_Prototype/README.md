# Dropflare — Unity Test 01

Small, original third-person Unity 6 prototype. No asset-store assets or paid dependencies.

## What's included
- A procedurally created 3D arena with walls, cover and floor grid
- A capsule player with follow camera
- Four stationary damageable targets (3 punches each)
- D-pad touchscreen controls and punch button
- Keyboard support: WASD / arrows and space
- Simple score and victory HUD

## Open and test
1. Install Unity Hub + Unity 6 editor, with Android Build Support if you intend to export an APK.
2. Open the folder `Dropflare_Unity_Prototype` as a Unity project.
3. In the Unity editor, open `Assets/Scenes/Main.unity`.
4. Press Play. The arena is built entirely by `DropflarePrototype.cs` at runtime.
5. For Android, add `Main.unity` to Build Profiles, select Android and build. Select landscape orientation.

## Important limitations
This project source was assembled without access to the Unity editor in the current runtime. It has **not** been imported, compiled, play-tested, or exported to APK. Unity may request package resolution or an editor version upgrade when opened. The prototype is an intentionally minimal technical experiment: no network multiplayer, store, matchmaking or original Dropflare characters yet. This is not a replacement of the existing Dropflare game.

Unity Personal eligibility and Unity editor installation: https://unity.com/products/unity-personal

## Cloud build preparation
See CLOUD-BUILD.md. Scene registration, IMGUI module and Android build settings are now included. No successful Unity compilation or APK has been produced yet.
