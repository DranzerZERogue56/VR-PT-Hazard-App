using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VRPT.Hazards
{
    // Singleton that holds the live session, accepts student tags, and on EndSession()
    // matches them against scene Hazards and writes a JSON report.
    public class SessionReporter : MonoBehaviour
    {
        public static SessionReporter Instance { get; private set; }

        [SerializeField] private string homeId = "home-001";
        [SerializeField] private string studentId = "anonymous";
        [SerializeField] private bool autoStartOnAwake = true;

        private SessionRecord record;
        private readonly HashSet<string> matchedHazardIds = new();
        private int tagCounter;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            if (autoStartOnAwake) StartSession();
        }

        public void StartSession()
        {
            record = new SessionRecord
            {
                sessionId = Guid.NewGuid().ToString(),
                studentId = studentId,
                homeId = homeId,
                startedAtUtc = DateTime.UtcNow.ToString("o")
            };
            matchedHazardIds.Clear();
            tagCounter = 0;
        }

        public void RegisterTag(Vector3 worldPos, HazardType studentType, string studentLabel)
        {
            if (record == null) StartSession();

            var hazards = FindObjectsOfType<Hazard>();
            Hazard bestMatch = null;
            float bestDist = float.MaxValue;
            foreach (var h in hazards)
            {
                float d = Vector3.Distance(h.transform.position, worldPos);
                if (d <= h.MatchRadius && d < bestDist)
                {
                    bestDist = d;
                    bestMatch = h;
                }
            }

            var entry = new TaggedHazardRecord
            {
                tagId = $"tag-{++tagCounter}",
                studentLabel = studentLabel,
                studentType = studentType,
                tagPosition = worldPos,
                matchedHazardId = bestMatch != null ? bestMatch.HazardId : null,
                isCorrectMatch = bestMatch != null && bestMatch.Type == studentType,
                distanceToMatch = bestMatch != null ? bestDist : -1f
            };
            record.taggedHazards.Add(entry);
            if (bestMatch != null) matchedHazardIds.Add(bestMatch.HazardId);
        }

        public string EndSessionAndSave()
        {
            if (record == null) return null;
            record.endedAtUtc = DateTime.UtcNow.ToString("o");

            foreach (var h in FindObjectsOfType<Hazard>())
            {
                if (matchedHazardIds.Contains(h.HazardId)) continue;
                record.missedHazards.Add(new MissedHazardRecord
                {
                    hazardId = h.HazardId,
                    type = h.Type,
                    severity = h.Severity,
                    position = h.transform.position
                });
            }

            record.truePositiveCount = 0;
            record.falsePositiveCount = 0;
            foreach (var t in record.taggedHazards)
            {
                if (t.isCorrectMatch) record.truePositiveCount++;
                else record.falsePositiveCount++;
            }
            record.falseNegativeCount = record.missedHazards.Count;

            string json = JsonUtility.ToJson(record, prettyPrint: true);
            string filename = $"VR-PT_project_session_{record.homeId}_{record.studentId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            string path = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllText(path, json);
            Debug.Log($"[SessionReporter] Report saved: {path}");
            return path;
        }

        private void OnApplicationQuit()
        {
            if (record != null && string.IsNullOrEmpty(record.endedAtUtc))
                EndSessionAndSave();
        }
    }
}
