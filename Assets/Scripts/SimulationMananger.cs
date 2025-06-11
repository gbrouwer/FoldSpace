using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    public ATCController centralATC;
    public ATCController spokeATC;
    public UNetController uNetController;
    public Transform droneParent;
    public GameObject volumeVisualPrefab;

    [Header("Drone Settings")]
    public GameObject dronePrefab;
    public int numberOfFlights = 5;
    public float spawnInterval = 2f;
    public float volSize = 5f;      // for time index calculation
    public float droneSpeed = 5f;   // for time index calculation

    private float timer;
    private Queue<FlightIntent> flightQueue = new();
    private List<FlightIntent> completedFlights = new();

    void Awake()
    {
        VolumeVisualizer.Initialize(volumeVisualPrefab);
    }

    void Start()
    {
        if (droneParent == null)
        {
            GameObject parentObj = GameObject.Find("Drones");
            if (parentObj != null) droneParent = parentObj.transform;
            else Debug.LogWarning("No Drones parent found in scene");
        }

        InitializeScenario();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (flightQueue.Count > 0 && timer >= spawnInterval)
        {
            timer = 0f;
            LaunchDrone(flightQueue.Dequeue());
        }
    }

    void InitializeScenario()
    {
        for (int i = 0; i < numberOfFlights; i++)
        {
            var intent = new FlightIntent
            {
                origin = spokeATC,
                destination = centralATC
            };
            flightQueue.Enqueue(intent);
        }
    }

    int GetCurrentTimeIndex()
    {
        return Mathf.FloorToInt(Time.time / (volSize / droneSpeed));
    }

    void LaunchDrone(FlightIntent intent)
    {
        int currentTimeIndex = GetCurrentTimeIndex();
        List<VolumeAgent> path = uNetController.AssignPath(intent.origin, intent.destination, currentTimeIndex);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Flight skipped — no available route.");
            return;
        }

        GameObject droneGO = Instantiate(dronePrefab, intent.origin.transform.position, Quaternion.identity, droneParent);
        DroneAgent agent = droneGO.GetComponent<DroneAgent>();
        if (agent == null)
        {
            Debug.LogError("Spawned drone prefab does not contain a DroneAgent component!");
            return;
        }

        agent.InitializePath(path.Select(v => v.worldPosition).ToList());
        intent.origin.NotifyTakeoff(agent);
        intent.spawnedAgent = agent;

        DroneAgentStatus status = droneGO.GetComponent<DroneAgentStatus>();
        if (status == null)
            status = droneGO.AddComponent<DroneAgentStatus>();

        status.SetFlightInfo(intent.flightID, path, path.First(), path.Last(), intent.callSign);

        // Attach camera
        GameObject camObj = new GameObject("DroneCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.enabled = false;
        DroneCameraFollower follower = camObj.AddComponent<DroneCameraFollower>();
        follower.SetTarget(droneGO.transform);

        // Register with camera manager
        CameraManager cm = Object.FindFirstObjectByType<CameraManager>();
        if (cm != null)
        {
            cm.RegisterDroneCamera(cam);
        }



        VolumeVisualizer.ShowVolumes(path);
    }
}
