# VR PT Hazard App

A Unity VR application for academic Physical Therapy courses. Students walk through 3D-scanned **real, furnished homes** in VR and identify fall hazards and accessibility issues. The instructor pre-places an answer key in the scene; the app records what the student tagged, what they missed, and exports a JSON session report.

## Quick start

```bash
git clone https://github.com/DranzerZERogue56/VR-PT-Hazard-App
```

1. Open **Unity Hub → Open → Add project from disk** → select the cloned folder.
2. Unity 2022.3.x will install on first open (Unity Hub will offer the right version automatically from `ProjectVersion.txt`). Required Editor modules: **Android Build Support** (Quest) and your desktop platform's IL2CPP module (PC VR).
3. When the project opens, Unity resolves all packages from `Packages/manifest.json` — OpenXR, XR Interaction Toolkit, Input System, URP, TextMeshPro. If prompted to import TMP Essentials, click Import.
4. Menu: **VRPT → Bootstrap Sample Scene & Prefabs**. This builds `Assets/Scenes/Sample_TestRoom.unity` with a primitive room, four hazards, and a tag-marker prefab.
5. Open the scene. Add an XR rig from the XRI Starter Assets samples (Package Manager → XR Interaction Toolkit → Samples → Starter Assets → Import → drag the `XR Origin (Action-based)` prefab into the scene). Attach `HazardTagger` to one controller.
6. Press Play. Use the XR Device Simulator (Window → XR → XR Device Simulator) if you don't have a headset connected.

## Repository contents

- **`tutorial.md`** — non-technical 10-step build guide.
- **`setup/UNITY_SETUP.md`** — detailed setup walkthrough.
- **`Assets/Scripts/`** — runtime scripts:
  - `HazardType.cs` — enums (HazardType, HazardSeverity)
  - `Hazard.cs` — instructor-placed marker, invisible at runtime
  - `SessionRecord.cs` — serializable data classes for the JSON report
  - `HazardTagger.cs` — controller raycast → places tag, calls reporter
  - `SessionReporter.cs` — singleton, matches tags ↔ hazards, writes JSON
  - `HazardSelectionMenu.cs` — wrist-mounted radial selector for hazard types
- **`Assets/Scripts/Editor/SampleSceneBuilder.cs`** — adds the **VRPT** menu with the bootstrap action.
- **`Packages/manifest.json`** — pinned package versions so the project opens consistently.

## Target platforms

- Meta Quest 2/3 (standalone, Android build)
- PC VR (Vive, Index, Quest Link, etc. via OpenXR)

Single Unity project, two build profiles.

## MVP scope

1 home, student persona only, walkthrough + hazard tagging + JSON report. Persona modes (elderly, wheelchair) and consequence simulation are planned for later phases.

## Use case

Built for academic PT instruction. Homes used in the app should be scanned with explicit homeowner consent; the `.gitignore` excludes scan files (`*.obj`, `*.glb`, `*.ply`, etc.) and session reports (`session_*.json`) to keep PII and large binaries out of git.

## License

TBD.
