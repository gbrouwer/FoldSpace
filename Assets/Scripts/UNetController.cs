using System.Collections.Generic;
using UnityEngine;

public class UNetController : MonoBehaviour
{
    [Header("Visualization")]
    public GameObject volumeVisualPrefab;

    private Dictionary<string, List<VolumeAgent>> flightReservations = new();

    void Start()
    {
        VolumeVisualizer.Initialize(volumeVisualPrefab);
    }

    public List<VolumeAgent> AssignPath(ATCController origin, ATCController destination, int startTimeIndex)
    {
        Vector3 originPos = origin.transform.position;
        Vector3 destPos = destination.transform.position;

        int wrappedT = VolumeUtils.WrapTimeIndex(startTimeIndex);

        VolumeAgent start = FindClosestAgent(originPos, wrappedT);
        VolumeAgent goal = FindClosestAgent(destPos, wrappedT);

        if (start == null || goal == null)
        {
            Debug.LogError("Start or goal VolumeAgent not found.");
            return null;
        }

        List<VolumeAgent> path = GraphSearchAlgorithms.AStar(start, goal);
        foreach (var node in path)
            Debug.Log($"Path includes: {node.gridPosition} at t={node.timeIndex}");


        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path found between origin and destination.");
            return null;
        }

        string flightID = System.Guid.NewGuid().ToString();
        bool reserved = ReserveVolumes(flightID, path);

        if (!reserved)
        {
            Debug.LogWarning("Reservation failed — path already in use.");
            return null;
        }

        flightReservations[flightID] = path;
        VisualizePath(flightID);
        return path;
    }

    public bool ReserveVolumes(string flightID, List<VolumeAgent> path)
    {
        foreach (var volume in path)
        {
            if (!volume.TryReserve(flightID))
                return false;
        }

        flightReservations[flightID] = path;
        return true;
    }

    public void ReleaseVolumes(string flightID)
    {
        if (flightReservations.TryGetValue(flightID, out var path))
        {
            foreach (var volume in path)
            {
                volume.ReleaseReservation(flightID);
            }
            flightReservations.Remove(flightID);
        }
    }

    public void VisualizePath(string flightID)
    {
        if (flightReservations.TryGetValue(flightID, out var path))
        {
            VolumeVisualizer.ShowVolumes(path);
        }
    }

    private VolumeAgent FindClosestAgent(Vector3 worldPos, int timeIndex)
    {
        float minDist = float.MaxValue;
        VolumeAgent closest = null;

        foreach (var pair in VolumeGridBuilder.graph.agents)
        {
            VolumeAgent agent = pair.Value;
            if (agent.timeIndex != timeIndex) continue;

            float dist = Vector3.Distance(worldPos, agent.worldPosition);
            if (dist < minDist)
            {
                minDist = dist;
                closest = agent;
            }
        }

        return closest;
    }
}