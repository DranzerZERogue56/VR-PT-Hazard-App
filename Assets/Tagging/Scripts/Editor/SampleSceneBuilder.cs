using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VRPT.Hazards;

namespace VRPT.HazardsEditor
{
    // Menu: VRPT -> Bootstrap Sample Scene & Prefabs
    // Builds a primitive test "room" with floor, walls, a rug, a cord, a low table,
    // 4 instructor-placed hazards, a SessionReporter, and a tag-marker prefab.
    // Use it to validate the loop before importing a real scan.
    public static class SampleSceneBuilder
    {
        const string SceneFolder  = "Assets/Scenes";
        const string PrefabFolder = "Assets/Prefabs";
        const string ScenePath    = SceneFolder  + "/Sample_TestRoom.unity";
        const string MarkerPath   = PrefabFolder + "/TagMarker.prefab";
        const string MarkerMatPath= PrefabFolder + "/TagMarker_Mat.mat";

        [MenuItem("VRPT/Bootstrap Sample Scene & Prefabs")]
        public static void Build()
        {
            EnsureFolder(SceneFolder);
            EnsureFolder(PrefabFolder);

            var markerPrefab = BuildTagMarkerPrefab();

            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            BuildRoom();
            BuildHazards();
            BuildSessionReporter();

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog(
                "VRPT Sample Scene",
                "Built:\n  " + ScenePath + "\n  " + MarkerPath +
                "\n\nOpen the scene and press Play. You'll need an XR rig (XRI Starter Assets sample) to actually tag in-headset.",
                "OK");
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = Path.GetDirectoryName(path).Replace('\\', '/');
            var leaf   = Path.GetFileName(path);
            if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }

        static GameObject BuildTagMarkerPrefab()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "TagMarker";
            go.transform.localScale = Vector3.one * 0.1f;
            Object.DestroyImmediate(go.GetComponent<Collider>());

            var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color");
            var mat = new Material(shader) { color = new Color(0.1f, 1f, 0.4f) };
            AssetDatabase.CreateAsset(mat, MarkerMatPath);
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, MarkerPath);
            Object.DestroyImmediate(go);
            return prefab;
        }

        static void BuildRoom()
        {
            var root = new GameObject("Room");

            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.SetParent(root.transform);
            floor.transform.localScale = new Vector3(0.8f, 1, 0.8f); // 8x8m

            Wall(root.transform, "Wall_N", new Vector3(0, 1.25f,  4f), new Vector3(8f,   2.5f, 0.1f));
            Wall(root.transform, "Wall_S", new Vector3(0, 1.25f, -4f), new Vector3(8f,   2.5f, 0.1f));
            Wall(root.transform, "Wall_E", new Vector3( 4f, 1.25f, 0), new Vector3(0.1f, 2.5f, 8f));
            Wall(root.transform, "Wall_W", new Vector3(-4f, 1.25f, 0), new Vector3(0.1f, 2.5f, 8f));

            var rug = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rug.name = "Rug";
            rug.transform.SetParent(root.transform);
            rug.transform.position    = new Vector3(0, 0.01f, 0);
            rug.transform.localScale  = new Vector3(2f, 0.02f, 1.5f);

            var cord = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cord.name = "ExtensionCord";
            cord.transform.SetParent(root.transform);
            cord.transform.position   = new Vector3(-1.5f, 0.02f, 2f);
            cord.transform.rotation   = Quaternion.Euler(0, 0, 90);
            cord.transform.localScale = new Vector3(0.02f, 1.5f, 0.02f);

            var table = GameObject.CreatePrimitive(PrimitiveType.Cube);
            table.name = "LowTable";
            table.transform.SetParent(root.transform);
            table.transform.position   = new Vector3(2f, 0.25f, -2f);
            table.transform.localScale = new Vector3(0.8f, 0.5f, 0.6f);
        }

        static void Wall(Transform parent, string name, Vector3 pos, Vector3 scale)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent);
            go.transform.position   = pos;
            go.transform.localScale = scale;
        }

        static void BuildHazards()
        {
            var parent = new GameObject("Hazards").transform;
            MakeHazard(parent, "Hazard_Rug",      new Vector3( 0,   0.05f,  0),   HazardType.LooseRug,       HazardSeverity.High,   "Rug with no anti-slip backing in main walkway.");
            MakeHazard(parent, "Hazard_Cord",     new Vector3(-1.5f,0.05f,  2),   HazardType.ElectricalCord, HazardSeverity.High,   "Extension cord runs across walkway.");
            MakeHazard(parent, "Hazard_Table",    new Vector3( 2,   0.55f, -2),   HazardType.ClutteredPath,  HazardSeverity.Medium, "Low table protrudes into pathway.");
            MakeHazard(parent, "Hazard_Lighting", new Vector3( 3,   1.5f,   3),   HazardType.PoorLighting,   HazardSeverity.Medium, "Corner is poorly lit at night.");
        }

        static void MakeHazard(Transform parent, string id, Vector3 pos, HazardType type, HazardSeverity sev, string desc)
        {
            var go = new GameObject(id);
            go.transform.SetParent(parent);
            go.transform.position = pos;

            var h  = go.AddComponent<Hazard>();
            var so = new SerializedObject(h);
            so.FindProperty("hazardId").stringValue       = id;
            so.FindProperty("type").enumValueIndex        = (int)type;
            so.FindProperty("severity").enumValueIndex    = (int)sev;
            so.FindProperty("description").stringValue    = desc;
            so.FindProperty("matchRadius").floatValue     = 1.0f;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void BuildSessionReporter()
        {
            var go = new GameObject("SessionReporter");
            go.AddComponent<SessionReporter>();
        }
    }
}
