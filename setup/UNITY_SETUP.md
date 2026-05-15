# Unity Setup — One-time bootstrap

Do this once on the machine that will build the app. ~30–45 minutes total, mostly waiting on downloads.

## 1. Install Unity Hub and the right editor

1. Download Unity Hub: https://unity.com/download
2. In Unity Hub → Installs → Install Editor → pick **Unity 2022.3 LTS** (any 2022.3.x is fine).
3. When prompted for modules, tick:
   - **Android Build Support** (and the **OpenJDK** + **Android SDK & NDK Tools** sub-items) — needed for Meta Quest builds.
   - **Windows Build Support (IL2CPP)** if you're on Windows, or **Mac Build Support (IL2CPP)** if on Mac — needed for PC VR.

## 2. Create the project

1. Unity Hub → Projects → New project.
2. Template: **3D (URP)** — Universal Render Pipeline. Do NOT pick "3D" (built-in pipeline) or HDRP.
3. Name: `VR-PT-Hazard-App`. Location: anywhere you like (e.g. `/home/benjamin/VR-PT-Hazard-App/UnityProject`).
4. Click Create. First open takes a few minutes.

## 3. Install required packages

In Unity: **Window → Package Manager**, change the dropdown at the top-left from "In Project" to **"Unity Registry"**. Install these one at a time:

- **XR Plugin Management** (search "XR Plugin")
- **OpenXR Plugin**
- **XR Interaction Toolkit** — when prompted to install Starter Assets/samples, say YES.
- **Input System** — when it asks to restart and switch to the new input system, say YES.

Then from Package Manager → click the **+** icon → **Add package by name** → enter:
- `com.unity.xr.meta-openxr` (Meta Quest support)

## 4. Configure XR

1. **Edit → Project Settings → XR Plug-in Management**.
2. On the **Windows/Mac tab**: tick **OpenXR**.
3. On the **Android tab**: tick **OpenXR** and **Meta Quest feature group**.
4. Under **XR Plug-in Management → OpenXR**, on both tabs:
   - Add interaction profiles: **Oculus Touch Controller Profile** (for Quest) and **Valve Index** / **HTC Vive** / **Microsoft Motion Controller** profiles as needed.
   - Fix any red exclamation marks Unity flags (click them, "Fix all").

## 5. Drop in the project scripts

Copy every `.cs` file from `VR-PT-Hazard-App/UnityScripts/` into your Unity project's `Assets/Scripts/` folder. Unity will compile them automatically. No errors should appear in the console — if any do, stop and let me know.

## 6. Build settings

**File → Build Settings**:
- For Quest builds: switch platform to **Android**, set Texture Compression to **ASTC**.
- For PC VR builds: switch platform to **Windows/Mac/Linux**.
- Save two **Build Profiles** (one for each) so you can flip without reconfiguring.

You can keep working on scenes/scripts before you have a scan. The scan is only needed at Step 3 of the tutorial (importing the home).
