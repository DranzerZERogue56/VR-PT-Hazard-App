# VR PT Hazard App — Working Tutorial

A non-technical, step-by-step guide for building and running the app. Each "step" captures a known gotcha or decision point so we don't lose the lesson later.

---

## Step 1 — Pick a home worth scanning

Choose a real, lived-in home with realistic clutter: rugs that curl at the edges, electrical cords across walkways, dim hallways, cluttered stairs, narrow bathroom doorways. **Empty staged homes will not work** — students need real hazards to find. Get verbal (and ideally written) permission from the homeowner before scanning.

## Step 2 — Check with the university about consent

Before any scan leaves your laptop, confirm whether your PT department or IRB needs a homeowner release form. Even though it's just architecture, a private home being shown to students is a privacy question. Ask once, in writing, so you have a record.

## Step 3 — Scan the home

Use an iPhone or iPad Pro with LiDAR + the Polycam app. Walk slowly, overlap your sweeps, and cover every room you want students to explore. Plan ~30–60 minutes per home. Export the scan as OBJ or GLB with textures.

> You don't need a scan to start building — Step 4 sets up a primitive "test room" so the rest of the app can be developed and validated first.

## Step 4 — Open the Unity project (one-click bootstrap)

The repo is a real Unity project, not just loose scripts. Clone it, then in Unity Hub click **Open → Add project from disk** and pick the cloned folder. Unity Hub will offer to install the correct Unity version (2022.3 LTS) — accept, and tick **Android Build Support** (for Quest) plus your desktop platform's build support (for PC VR). Unity then resolves all required packages automatically. First open takes ~10 minutes, mostly downloads.

## Step 5 — Build the test scene from the menu

In Unity's top menu bar: **VRPT → Bootstrap Sample Scene & Prefabs**. One click. This creates a small primitive room (floor, walls, a rug, a cord across a walkway, a low table) with four hazards pre-placed and an invisible scoring system already wired up. Open the scene and you have a working app — no real scan required yet.

## Step 6 — Add the VR controls

Unity's XR Interaction Toolkit ships an "XR Origin" rig you drag into the scene. Open Package Manager, find XR Interaction Toolkit, expand Samples, click Import on **Starter Assets**, then drag the `XR Origin (Action-based)` prefab into the test scene. Add the `HazardTagger` script to one of the controller GameObjects, drop the tag-marker prefab into its inspector slot, and you can now tag hazards by aiming and pulling the trigger.

## Step 7 — Test without a headset

You don't need a Quest to validate the loop. **Window → XR → XR Device Simulator**, drag the simulator prefab into the scene, press Play. Mouse and keyboard now drive a fake headset and controllers. Walk up to the rug, aim, click. Quit Play mode, find the JSON report in `Application.persistentDataPath` (Unity will print the exact path in the Console). That confirms the whole pipeline works before you spend an hour on a real scan.

## Step 8 — Replace the test room with a real scanned home

Once you have a Polycam export and the test loop works, delete the `Room` GameObject in the sample scene and drag your scan in instead. Then walk through the imported home in Unity and reposition the four `Hazard` markers (and add more) onto real hazards in the scan. The student doesn't see these markers — they're the instructor's answer key.

> Heads up: the next four steps are the gotchas you'll hit with real scans.

## Step 9 — Expect the floor to be uneven

Photogrammetric scans almost never produce a perfectly flat floor. When you bring the scan into Unity, the player will feel like they're walking on a rumpled rug. **The fix:** add an invisible flat floor for each room that the player actually walks on. The visible scan stays, but movement happens on the flat layer underneath.

## Step 10 — Turn off extra lighting

The lighting from the real house is already "painted" into the scan's textures (sun through windows, lamp glow, shadows). If Unity adds its own sunlight on top, the scene looks washed out and double-lit. **The fix:** disable or dim Unity's default directional light after importing the scan.

## Step 11 — Shrink the scan before loading it on the headset

A Meta Quest can only handle so much detail at once. A raw scan is usually too heavy and will stutter or crash. **The fix:** reduce the scan's detail (Polycam offers lower-detail export options, or simplify it in Unity/Blender) until it runs smoothly on the headset. PC VR can handle the full-detail version.

## Step 12 — Build for the headset

Build the app twice from the same project: once for Meta Quest (set platform to Android, texture compression ASTC), once for PC VR (Windows/Mac/Linux). Save both as Build Profiles so you can flip between them. Test locomotion in-headset; if students feel motion sick, switch the default move type from smooth-walking to teleport.

## Step 13 — Collect the session report

When a student finishes a walkthrough, the app saves a JSON file listing which hazards they correctly tagged, which they missed, and any extras they flagged by mistake. On Quest, pull the file off the headset over USB (`adb pull`). On PC, it lands in a known folder Unity prints to the Console on quit. The instructor reviews these to grade or debrief.

## Step 14 — Debrief, then iterate

After the first class session, note which hazards students consistently miss and which they over-tag. That tells us where the scan needs better lighting, where a hazard marker is in the wrong spot, or where the student briefing needs more detail. Update the markers in Unity and rebuild.

---

## Future phases (not in MVP)

- **Elderly persona** — slower walk, screen vignette + audio cue + logged "incident" when a missed trip hazard is crossed.
- **Wheelchair persona** — seated rig, blocked by narrow doorways and high thresholds.
- **More homes** — repeat steps 1–8 for each.
- **In-VR hazard authoring** — let instructors place hazards without opening Unity.
- **HTML report viewer** — single static page that renders the JSON session report (score, missed hazards on a floorplan, timeline) so instructors don't read raw JSON.
- **Desktop (non-VR) walkthrough build** — same scripts, first-person controller instead of XR rig, for students without headset access.
