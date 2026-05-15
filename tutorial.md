# VR PT Hazard App — Working Tutorial

A non-technical, step-by-step guide for building and running the app. Each "step" captures a known gotcha or decision point so we don't lose the lesson later.

---

## Step 1 — Pick a home worth scanning

Choose a real, lived-in home with realistic clutter: rugs that curl at the edges, electrical cords across walkways, dim hallways, cluttered stairs, narrow bathroom doorways. **Empty staged homes will not work** — students need real hazards to find. Get verbal (and ideally written) permission from the homeowner before scanning.

## Step 2 — Check with the university about consent

Before any scan leaves your laptop, confirm whether your PT department or IRB needs a homeowner release form. Even though it's just architecture, a private home being shown to students is a privacy question. Ask once, in writing, so you have a record.

## Step 3 — Scan the home

Use an iPhone or iPad Pro with LiDAR + the Polycam app. Walk slowly, overlap your sweeps, and cover every room you want students to explore. Plan ~30–60 minutes per home. Export the scan as OBJ or GLB with textures.

## Step 4 — Expect the floor to be uneven

Photogrammetric scans almost never produce a perfectly flat floor. When you bring the scan into Unity, the player will feel like they're walking on a rumpled rug. **The fix:** add an invisible flat floor for each room that the player actually walks on. The visible scan stays, but movement happens on the flat layer underneath.

## Step 5 — Turn off extra lighting

The lighting from the real house is already "painted" into the scan's textures (sun through windows, lamp glow, shadows). If Unity adds its own sunlight on top, the scene looks washed out and double-lit. **The fix:** disable or dim Unity's default directional light after importing the scan.

## Step 6 — Shrink the scan before loading it on the headset

A Meta Quest can only handle so much detail at once. A raw scan is usually too heavy and will stutter or crash. **The fix:** reduce the scan's detail (Polycam offers lower-detail export options, or we can simplify it in Unity/Blender) until it runs smoothly on the headset. PC VR can handle the full-detail version.

## Step 7 — Place hazards in the scene

Walk through the imported home in Unity and mark each hazard (trip, reach, lighting, threshold, etc.) with a small invisible marker. This is the instructor's "answer key" — students don't see these markers, but the app uses them to score whether students tagged the right spots.

## Step 8 — Build for the headset

Build the app twice from the same project: once for Meta Quest (standalone), once for PC VR. Test both. Locomotion should feel comfortable — use smooth walking with snap turning, and offer teleport as a backup for students who get motion sick.

## Step 9 — Collect the session report

When a student finishes a walkthrough, the app saves a report file listing which hazards they correctly tagged, which they missed, and any extras they flagged by mistake. On Quest, pull the file off the headset over USB. On PC, it lands in a known folder on disk. The instructor reviews these to grade or debrief.

## Step 10 — Debrief, then iterate

After the first class session, note which hazards students consistently miss and which they over-tag. That tells us where the scan needs better lighting, where a hazard marker is in the wrong spot, or where the student briefing needs more detail. Update and rebuild.

---

## Future phases (not in MVP)

- **Elderly persona** — slower walk, screen vignette + audio cue + logged "incident" when a missed trip hazard is crossed.
- **Wheelchair persona** — seated rig, blocked by narrow doorways and high thresholds.
- **More homes** — repeat steps 1–7 for each.
- **In-VR hazard authoring** — let instructors place hazards without opening Unity.
