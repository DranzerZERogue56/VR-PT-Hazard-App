# Unity Setup

One-time bootstrap for the machine that builds the app. With the repo cloned, this should take ~10 minutes (mostly Unity install).

## 1. Install Unity Hub and the right editor

1. Download Unity Hub: https://unity.com/download
2. Clone this repo and open it in Unity Hub (**Open → Add project from disk**).
3. Unity Hub reads `ProjectSettings/ProjectVersion.txt` and offers to install **Unity 2022.3.40f1** (or close — any 2022.3 LTS works). Accept.
4. When prompted for modules, tick:
   - **Android Build Support** (with **OpenJDK** + **Android SDK & NDK Tools**) — needed for Meta Quest builds.
   - **Windows Build Support (IL2CPP)** or **Mac Build Support (IL2CPP)** — needed for PC VR.

## 2. First open

Unity resolves packages from `Packages/manifest.json` automatically — OpenXR, XR Interaction Toolkit, Input System, URP, TextMeshPro.

You may see these prompts on first open — accept all:
- **TMP Essentials Import** → Import.
- **Restart for new Input System** → Restart.
- **Update XR packages** → leave at pinned versions for now.

## 3. Configure XR (one-time)

**Edit → Project Settings → XR Plug-in Management**.
- **Windows/Mac tab**: tick **OpenXR**.
- **Android tab**: tick **OpenXR** and (if available) **Meta Quest feature group**.

Under **XR Plug-in Management → OpenXR**, on both tabs:
- Add interaction profiles: **Oculus Touch Controller Profile** for Quest; **Valve Index** / **HTC Vive** / **Microsoft Motion Controller** for PC VR as needed.
- Fix any red exclamation marks Unity flags ("Fix all").

## 4. Bootstrap the sample scene

Menu: **VRPT → Bootstrap Sample Scene & Prefabs**.

This creates:
- `Assets/Scenes/Sample_TestRoom.unity` — primitive room with 4 hazards and a `SessionReporter`.
- `Assets/Prefabs/TagMarker.prefab` — small green sphere placed when the student tags.

Open the scene. It does NOT yet contain an XR rig — that has to come from the XRI samples (next step).

## 5. Add an XR rig from XRI Starter Assets

1. **Window → Package Manager → XR Interaction Toolkit → Samples → Starter Assets → Import**.
2. From `Assets/Samples/.../Starter Assets/Prefabs/`, drag **XR Origin (Action-based)** into the scene.
3. Add the `HazardTagger` component to one of the controller GameObjects under the rig. Wire:
   - **Ray Origin** → the controller transform.
   - **Tag Action** → reference the trigger action from the XRI Starter Assets input actions asset.
   - **Tag Marker Prefab** → drag in `Assets/Prefabs/TagMarker.prefab`.

## 6. Test without a headset

**Window → XR → XR Device Simulator**. Drag the simulator prefab from the XRI samples into the scene; you can now move/aim/click with mouse + keyboard in Play mode.

## 7. Build settings

**File → Build Settings**:
- For Quest builds: switch platform to **Android**, set Texture Compression to **ASTC**, add `Sample_TestRoom` to Scenes In Build.
- For PC VR builds: switch platform to **Windows/Mac/Linux**.
- Save two **Build Profiles** (one per target) so you can flip without reconfiguring.

You can keep working on scenes/scripts before you have a real home scan. The scan is only needed when you're ready to replace the primitive `Room` GameObject in the sample scene.
