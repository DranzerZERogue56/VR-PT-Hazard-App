using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRPT.Hazards
{
    // Wrist-mounted radial selector. Attach to a world-space Canvas parented to the
    // off-hand controller. At Start it auto-generates one button per HazardType,
    // arranged in a circle, and wires each to HazardTagger.SetSelection.
    //
    // Scene setup (one-time, in Unity):
    //   1. Right-click off-hand controller -> UI -> Canvas. Set Render Mode = World Space.
    //   2. Scale the Canvas to ~0.002, position ~0.05m above the wrist, rotate to face the player.
    //   3. Add this component to the Canvas. Assign the HazardTagger reference.
    //   4. (Optional) Assign a simple Button prefab with a TMP_Text child named "Label".
    //      If left empty, a plain runtime button is generated.
    public class HazardSelectionMenu : MonoBehaviour
    {
        [SerializeField] private HazardTagger tagger;
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private TMP_Text currentSelectionLabel;
        [SerializeField] private float radius = 120f;

        private HazardType selected = HazardType.TripHazard;

        private void Start()
        {
            BuildButtons();
            ApplySelection(selected);
        }

        private void BuildButtons()
        {
            var container = buttonContainer != null ? buttonContainer : (RectTransform)transform;
            var types = (HazardType[])Enum.GetValues(typeof(HazardType));

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

                btn.onClick.AddListener(() => ApplySelection(t));
            }
        }

        private void ApplySelection(HazardType t)
        {
            selected = t;
            if (tagger != null) tagger.SetSelection(t);
            if (currentSelectionLabel != null) currentSelectionLabel.text = Prettify(t.ToString());
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
            go.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.85f);

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
