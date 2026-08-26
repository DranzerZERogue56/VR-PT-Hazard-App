using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace VRPT.Hazards
{
    // Wrist-mounted radial selector. Attach to a world-space Canvas parented to the
    // off-hand controller. At Start it auto-generates one button per HazardType,
    // arranged in a circle, and wires each to HazardTagger.SetSelection. The active
    // selection also advances on Cycle Action (e.g. off-hand thumbstick click); whatever
    // is selected when HazardTagger's trigger action fires is the type that gets tagged.
    //
    // Scene setup (one-time, in Unity):
    //   1. Right-click off-hand controller -> UI -> Canvas. Set Render Mode = World Space.
    //   2. Scale the Canvas to ~0.002, position ~0.05m above the wrist, rotate to face the player.
    //   3. Add this component to the Canvas. Assign the HazardTagger reference.
    //   4. Assign Cycle Action to an InputActionReference bound to the off-hand
    //      controller's thumbstick click (e.g. <XRController>{LeftHand}/{Primary2DAxisClick}).
    //   5. (Optional) Assign a simple Button prefab with a TMP_Text child named "Label".
    //      If left empty, a plain runtime button is generated.
    public class HazardSelectionMenu : MonoBehaviour
    {
        [SerializeField] private HazardTagger tagger;
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private TMP_Text currentSelectionLabel;
        [SerializeField] private float radius = 120f;

        [Header("Cycle Input")]
        [SerializeField] private InputActionReference cycleAction;

        [Header("Selection Highlight")]
        [SerializeField] private Color normalColor = new Color(0.1f, 0.1f, 0.1f, 0.85f);
        [SerializeField] private Color selectedColor = new Color(0.16f, 0.45f, 0.85f, 0.95f);

        private HazardType selected = HazardType.TripHazard;
        private HazardType[] types;
        private readonly Dictionary<HazardType, Image> buttonGraphics = new Dictionary<HazardType, Image>();

        

        private void Start()
        {
            BuildButtons();
            ApplySelection(selected);
            Debug.Log("HazardSelectionMenu Script Loaded...", this);
        }

        private void OnEnable()
        {
            if (cycleAction != null)
            {
                cycleAction.action.performed += OnCyclePressed;
                cycleAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (cycleAction != null) cycleAction.action.performed -= OnCyclePressed;
        }

        private void OnCyclePressed(InputAction.CallbackContext _)
        {
            if (types == null || types.Length == 0) return;
            int index = Array.IndexOf(types, selected);
            int next = (index + 1) % types.Length;
            ApplySelection(types[next]);
            Debug.Log("<<JOYSTICK PRESSED>>", this);
            Debug.Log("Current Hazard: " + selected, this);
        }

        private void BuildButtons()
        {
            var container = buttonContainer != null ? buttonContainer : (RectTransform)transform;
            types = (HazardType[])Enum.GetValues(typeof(HazardType));
            buttonGraphics.Clear();

            for (int i = 0; i < types.Length; i++)
            {
                HazardType t = types[i];
                Button btn = buttonPrefab != null
                    ? Instantiate(buttonPrefab, container)
                    : CreateFallbackButton(container, t.ToString());

                float angle = (i / (float)types.Length) * Mathf.PI * 2f - Mathf.PI / 2f;
                var rt = (RectTransform)btn.transform;
                rt.anchoredPosition = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

                var label = btn.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = Prettify(t.ToString());

                var graphic = btn.GetComponent<Image>();
                if (graphic != null) buttonGraphics[t] = graphic;

                btn.onClick.AddListener(() => ApplySelection(t));
            }
        }

        private void ApplySelection(HazardType t)
        {
            selected = t;
            if (tagger != null) tagger.SetSelection(t);
            if (currentSelectionLabel != null) currentSelectionLabel.text = Prettify(t.ToString());

            foreach (var kvp in buttonGraphics)
                kvp.Value.color = kvp.Key == t ? selectedColor : normalColor;
        }

        private static string Prettify(string camel)
        {
            var sb = new System.Text.StringBuilder(camel.Length + 4);
            for (int i = 0; i < camel.Length; i++)
            {
                if (i > 0 && char.IsUpper(camel[i])) sb.Append(' ');
                sb.Append(camel[i]);
            }
            return sb.ToString();
        }

        private Button CreateFallbackButton(Transform parent, string label)
        {
            var go = new GameObject($"Btn_{label}", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            ((RectTransform)go.transform).sizeDelta = new Vector2(90, 30);
            go.GetComponent<Image>().color = normalColor;

            var textGo = new GameObject("Label", typeof(RectTransform));
            textGo.transform.SetParent(go.transform, false);
            var rt = (RectTransform)textGo.transform;
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 12;
            tmp.color = Color.white;

            return go.GetComponent<Button>();
        }
    }
}
