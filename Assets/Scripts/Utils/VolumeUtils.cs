using UnityEngine;

public static class VolumeUtils
{
    public static VolumeAgent FindClosestAgent(Vector3 worldPos, int timeIndex)
    {
        int wrappedTime = WrapTimeIndex(timeIndex);
        return FindClosestAgentAboveHeight(worldPos, wrappedTime, float.NegativeInfinity);
    }

    public static VolumeAgent FindClosestAgentAboveHeight(Vector3 worldPos, int timeIndex, float minY)
    {
        int wrappedTime = WrapTimeIndex(timeIndex);
        float minDist = float.MaxValue;
        VolumeAgent closest = null;

        for (int x = 0; x < VolumeGridBuilder.graph.countX; x++)
        for (int y = 0; y < VolumeGridBuilder.graph.countY; y++)
        for (int z = 0; z < VolumeGridBuilder.graph.countZ; z++)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            VolumeAgent agent = VolumeGridBuilder.graph.GetOrCreateAgent(pos, wrappedTime);
            if (agent == null || agent.worldPosition.y < minY)
                continue;

            float dist = Vector3.Distance(worldPos, agent.worldPosition);
            if (dist < minDist)
            {
                minDist = dist;
                closest = agent;
            }
        }

        return closest;
    }

    public static int WrapTimeIndex(int t)
    {
        return ((t % VolumeGridBuilder.graph.totalTimeSteps) + VolumeGridBuilder.graph.totalTimeSteps) % VolumeGridBuilder.graph.totalTimeSteps;
    }
} 
