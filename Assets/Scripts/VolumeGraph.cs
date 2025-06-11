using System.Collections.Generic;
using UnityEngine;

public class VolumeGraph
{
    public Dictionary<(Vector3Int, int), VolumeAgent> agents = new();
    public int totalTimeSteps;

    // Spatial bounds (set externally)
    public int countX;
    public int countY;
    public int countZ;
    public Vector3 gridOrigin;
    public float volSize;
    public float globalOffsetY;

    public void AddAgent(VolumeAgent agent)
    {
        agents[(agent.gridPosition, agent.timeIndex)] = agent;
    }

    public VolumeAgent GetAgent(Vector3Int pos, int t)
    {
        agents.TryGetValue((pos, t), out var agent);
        return agent;
    }

    public VolumeAgent GetOrCreateAgent(Vector3Int pos, int t)
    {
        var key = (pos, t);
        if (agents.TryGetValue(key, out var existing))
            return existing;

        // Bounds check
        if (pos.x < 0 || pos.x >= countX ||
            pos.y < 0 || pos.y >= countY ||
            pos.z < 0 || pos.z >= countZ ||
            t < 0 || t >= totalTimeSteps)
            return null;

        float offsetX = (pos.x * volSize) - ((countX - 1) * 0.5f * volSize);
        float offsetY = (pos.y * volSize) - ((countY - 1) * 0.5f * volSize) + globalOffsetY;
        float offsetZ = (pos.z * volSize) - ((countZ - 1) * 0.5f * volSize);
        Vector3 worldPos = gridOrigin + new Vector3(offsetX, offsetY, offsetZ);

        Terrain terrain = Terrain.activeTerrain;
        float terrainHeight = terrain.SampleHeight(worldPos) + terrain.transform.position.y;
        float volumeBottom = worldPos.y - (volSize * 0.5f);
        if (volumeBottom < terrainHeight)
            return null;

        VolumeAgent agent = new VolumeAgent(pos, t, worldPos);
        agents[key] = agent;
        return agent;
    }

    public List<VolumeAgent> GetNeighbors(VolumeAgent agent)
    {
        List<VolumeAgent> neighbors = new();
        Vector3Int[] spatialDirs = {
            Vector3Int.right, Vector3Int.left,
            Vector3Int.up, Vector3Int.down,
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };

        int nextT = (agent.timeIndex + 1) % totalTimeSteps;

        foreach (var dir in spatialDirs)
        {
            Vector3Int neighborPos = agent.gridPosition + dir;
            VolumeAgent neighbor = GetOrCreateAgent(neighborPos, nextT);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
                agent.AddNeighbor(neighbor, volSize / VolumeGridBuilder.GlobalDroneSpeed);
            }
        }

        return neighbors;
    }
}
