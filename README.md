# ENAMODS - BBQ Tools (MelonLoader mod for **ENA Dream BBQ**)

![Banner](https://github.com/realTobby/ENAMODS/blob/main/pics/ingame-freecam-ui-show.PNG)

Lightweight utility mod with a minimal HUD, and a fast FreeCam.

## Features
- **HUD (F9)** – scene + player position with RGB axes (X=red, Y=green, Z=blue).
- **FreeCam (F1)** – noclip on the player root; mouse wheel adjusts speed.

## Requirements
- **MelonLoader 0.7.x** (Open Beta)  
- Game: **ENA Dream BBQ** (Unity 2021.3, Mono / .NET 4.7.*)

## Install
1. Install MelonLoader to the game.
2. Drop `ENAMODS.dll` into `…/ENA Dream BBQ/Mods/`.
3. Launch the game.

## Controls
- **F1** – Toggle FreeCam (W/A/S/D + Space/Ctrl; **Scroll** = speed)
- **F9** – Toggle HUD

## Notes
- While FreeCam is ON, the game’s mover is disabled to avoid fighting physics.
- No speed hacks or other gameplay changes are included.

---

## Build (references)

Target framework: **.NET Framework 4.7** (4.7.2/4.8).  

Add references **from the game folder** (not the Unity Editor):

**MelonLoader (from `<Game>\MelonLoader\net35`):**
- `MelonLoader.dll`
- `0Harmony.dll` *(HarmonyLib)*

**Unity (from `<Game>\ENA-4-DreamBBQ_Data\Managed\`):**
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll`
- `UnityEngine.UIModule.dll`
- `UnityEngine.IMGUIModule.dll`
- `UnityEngine.InputLegacyModule.dll`
- `UnityEngine.PhysicsModule.dll`
- `UnityEngine.TextRenderingModule.dll`
- `UnityEngine.dll` *(if present)*

**Optional (instead of the local 0Harmony.dll):**
- NuGet package **HarmonyX** / **HarmonyLib** (keep only one Harmony reference).
