using System.Collections.Generic;
using UnityEngine;

public class VolumeAgent
{
    public Vector3Int gridPosition; // x, y, z
    public int timeIndex;           // t
    public Vector3 worldPosition;   // center in Unity world space
    public Vector3 scale = Vector3.one; // customizable scale (default 1x1x1)

    public bool isOccupied = false;
    public string reservedByID = null;

    public List<VolumeAgent> neighbors = new List<VolumeAgent>(); // for structure
    public List<VolumeEdge> edges = new List<VolumeEdge>();       // for cost-based routing

    public VolumeAgent(Vector3Int gridPos, int t, Vector3 worldPos)
    {
        gridPosition = gridPos;
        timeIndex = t;
        worldPosition = worldPos;
    }

    public void AddNeighbor(VolumeAgent neighbor, float weight = 1f)
    {
        if (!neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
        }
        edges.Add(new VolumeEdge(this, neighbor, weight));
    }

    public bool TryReserve(string flightID)
    {
        if (isOccupied)
            return false;

        isOccupied = true;
        reservedByID = flightID;
        return true;
    }

    public void ReleaseReservation(string flightID)
    {
        if (reservedByID == flightID)
        {
            isOccupied = false;
            reservedByID = null;
        }
    }
}
