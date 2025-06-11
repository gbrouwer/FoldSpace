using UnityEngine;

public class TestVisualizer : MonoBehaviour
{
    public ReachabilityVisualizer visualizer;

    [Header("Origin Volume Coordinates")]
    public Vector3Int originGrid = new Vector3Int(5, 2, 5);
    public int originTime = 0;

    void Start()
    {
        if (visualizer == null)
        {
            visualizer = Object.FindFirstObjectByType<ReachabilityVisualizer>();
            if (visualizer == null)
            {
                Debug.LogError("ReachabilityVisualizer not found in scene.");
                return;
            }
        }

        // Replace this with your actual scene location — e.g. Central Hub
        Vector3 testPos = new Vector3(150f, 0f, 0f);

        VolumeAgent origin = VolumeUtils.FindClosestAgent(testPos, 0);

        if (origin == null)
        {
            Debug.LogError($"No VolumeAgent found near {testPos} @ t=0");
            return;
        }

        visualizer.origin = origin;
        visualizer.Visualize();

        Debug.Log($"Reachability visualized from {origin.gridPosition} @ t={origin.timeIndex}");
    }

}
