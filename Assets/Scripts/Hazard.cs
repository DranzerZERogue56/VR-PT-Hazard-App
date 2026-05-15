using UnityEngine;

namespace VRPT.Hazards
{
    // Instructor-placed in the Unity Editor. Invisible to the student at runtime.
    // Each scene populates SessionReporter via OnEnable so the answer key is auto-discovered.
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private string hazardId;
        [SerializeField] private HazardType type;
        [SerializeField] private HazardSeverity severity = HazardSeverity.Medium;
        [TextArea, SerializeField] private string description;
        [Tooltip("Tag must be placed within this radius (meters) to count as a correct match.")]
        [SerializeField] private float matchRadius = 1.0f;

        public string HazardId => string.IsNullOrEmpty(hazardId) ? name : hazardId;
        public HazardType Type => type;
        public HazardSeverity Severity => severity;
        public string Description => description;
        public float MatchRadius => matchRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.4f, 0f, 0.35f);
            Gizmos.DrawSphere(transform.position, 0.15f);
            Gizmos.color = new Color(1f, 0.4f, 0f, 0.15f);
            Gizmos.DrawWireSphere(transform.position, matchRadius);
        }
    }
}
