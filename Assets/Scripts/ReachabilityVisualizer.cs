using System.Collections.Generic;
using UnityEngine;

public class ReachabilityVisualizer : MonoBehaviour
{
    public GameObject volumeVisualPrefab;
    public Transform visualizationRoot;
    public VolumeAgent origin;
    public bool useDijkstra = false;

    [ContextMenu("Visualize Reachability")]
    public void Visualize()
    {
        if (volumeVisualPrefab == null || origin == null || visualizationRoot == null)
        {
            Debug.LogWarning("Missing reference(s) in ReachabilityVisualizer");
            return;
        }

        foreach (Transform child in visualizationRoot)
        {
            DestroyImmediate(child.gameObject);
        }

        Dictionary<VolumeAgent, float> reachable = useDijkstra
            ? GraphSearchAlgorithms.Dijkstra(origin)
            : Convert(GraphSearchAlgorithms.BreadthFirstSearch(origin));

        float maxTime = 0f;
        foreach (var val in reachable.Values)
            if (val > maxTime) maxTime = val;

        foreach (var pair in reachable)
        {
            VolumeAgent agent = pair.Key;
            float t = pair.Value / maxTime;
            Color c = Color.Lerp(Color.blue, Color.red, t);

            GameObject visual = Instantiate(volumeVisualPrefab, agent.worldPosition, Quaternion.identity, visualizationRoot);
            visual.name = $"Reachable_{agent.gridPosition}_t{agent.timeIndex}";
            var renderer = visual.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Clone the material to ensure per-volume instance
                Material matInstance = new Material(renderer.sharedMaterial);

                // Set base and emission colors
                matInstance.SetColor("_BaseColor", c);
                matInstance.SetColor("_EmissionColor", c * 2f); // Optional: boost intensity

                // Ensure emission is actually turned on
                matInstance.EnableKeyword("_EMISSION");

                // Apply to the volume
                renderer.material = matInstance;
            }
        }
    }

    private Dictionary<VolumeAgent, float> Convert(Dictionary<VolumeAgent, int> input)
    {
        var result = new Dictionary<VolumeAgent, float>();
        foreach (var pair in input)
            result[pair.Key] = pair.Value;
        return result;
    }
}
