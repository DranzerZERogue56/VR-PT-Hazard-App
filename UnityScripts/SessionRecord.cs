using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRPT.Hazards
{
    [Serializable]
    public class SessionRecord
    {
        public string sessionId;
        public string studentId;
        public string homeId;
        public string startedAtUtc;
        public string endedAtUtc;
        public List<TaggedHazardRecord> taggedHazards = new();
        public List<MissedHazardRecord> missedHazards = new();
        public int truePositiveCount;
        public int falsePositiveCount;
        public int falseNegativeCount;
    }

    [Serializable]
    public class TaggedHazardRecord
    {
        public string tagId;
        public string studentLabel;
        public HazardType studentType;
        public Vector3 tagPosition;
        public string matchedHazardId;
        public bool isCorrectMatch;
        public float distanceToMatch;
    }

    [Serializable]
    public class MissedHazardRecord
    {
        public string hazardId;
        public HazardType type;
        public HazardSeverity severity;
        public Vector3 position;
    }
}
