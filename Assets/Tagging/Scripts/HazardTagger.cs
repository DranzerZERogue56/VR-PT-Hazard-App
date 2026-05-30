using UnityEngine;
using UnityEngine.InputSystem;

namespace VRPT.Hazards
{
    // Attach to one of the XR controllers. Raycasts forward; on trigger press, places
    // a HazardTag GameObject at the hit point and registers it with SessionReporter.
    public class HazardTagger : MonoBehaviour
    {
        [SerializeField] private InputActionReference tagAction;
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private GameObject tagMarkerPrefab;
        [SerializeField] private float maxRayDistance = 8f;
        [SerializeField] private LayerMask hitMask = ~0;
        [SerializeField] private HazardType currentSelection = HazardType.TripHazard;

        public void SetSelection(HazardType type) => currentSelection = type;

        private void OnEnable()
        {
            if (tagAction != null)
            {
                tagAction.action.performed += OnTagPressed;
                tagAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (tagAction != null) tagAction.action.performed -= OnTagPressed;
        }

        private void OnTagPressed(InputAction.CallbackContext _)
        {
            var origin = rayOrigin != null ? rayOrigin : transform;
            if (!Physics.Raycast(origin.position, origin.forward, out var hit, maxRayDistance, hitMask))
                return;

            if (tagMarkerPrefab != null)
                Instantiate(tagMarkerPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            SessionReporter.Instance?.RegisterTag(hit.point, currentSelection, currentSelection.ToString());
        }
    }
}
