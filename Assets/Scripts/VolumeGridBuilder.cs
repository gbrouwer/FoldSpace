using UnityEngine;

public class VolumeGridBuilder : MonoBehaviour
{
    [Header("Grid Dimensions")]
    public int countX = 10;
    public int countY = 5;
    public int countZ = 10;
    public int timeSteps = 6;

    [Header("Volume Settings")]
    public float volSize = 5f; // size of each volume in meters (assumed cubic)
    public float timeStepDuration = 1f;
    public float droneSpeed = 5f;
    public GameObject volumeVisualPrefab; // Optional prefab for debug visuals
    public bool visualize = false;
    public float globalOffsetY = 10f;

    public static float GlobalDroneSpeed;
    public static VolumeGraph graph;

    void Awake()
    {
        GlobalDroneSpeed = droneSpeed;
        graph = new VolumeGraph();

        graph.countX = countX;
        graph.countY = countY;
        graph.countZ = countZ;
        graph.totalTimeSteps = timeSteps;
        graph.volSize = volSize;
        graph.globalOffsetY = globalOffsetY;
        graph.gridOrigin = transform.position;

        Debug.Log("VolumeGridBuilder initialized with lazy instantiation.");
    }
} 
