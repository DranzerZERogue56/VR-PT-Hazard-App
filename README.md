# VR PT Hazard App

A Unity VR application for academic Physical Therapy courses. Students walk through 3D-scanned **real, furnished homes** in VR and identify fall hazards and accessibility issues. The instructor pre-places an answer key in the scene; the app records what the student tagged, what they missed, and exports a JSON session report.

## Status

Early scaffold. No Unity project committed yet — only the scripts, setup guide, and tutorial.

## Repository contents

- **`tutorial.md`** — non-technical 10-step build guide.
- **`setup/UNITY_SETUP.md`** — one-time Unity Hub + package + XR configuration walkthrough.
- **`UnityScripts/`** — drop these `.cs` files into `Assets/Scripts/` of your Unity project:
  - `HazardType.cs` — enums (HazardType, HazardSeverity)
  - `Hazard.cs` — instructor-placed marker, invisible at runtime
  - `SessionRecord.cs` — serializable data classes for the JSON report
  - `HazardTagger.cs` — controller raycast → places tag, calls reporter
  - `SessionReporter.cs` — singleton, matches tags ↔ hazards, writes JSON
  - `HazardSelectionMenu.cs` — wrist-mounted radial selector for hazard types

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
