using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class GraphSearchAlgorithms
{
    public static List<VolumeAgent> AStar(VolumeAgent start, VolumeAgent goal)
    {
        var openSet = new SimplePriorityQueue<VolumeAgent>();
        var cameFrom = new Dictionary<VolumeAgent, VolumeAgent>();
        var gScore = new Dictionary<VolumeAgent, float>();
        var fScore = new Dictionary<VolumeAgent, float>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            VolumeAgent current = openSet.Dequeue();
            if (current == goal)
                return ReconstructPath(cameFrom, current);

            List<VolumeAgent> neighbors = VolumeGridBuilder.graph.GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                float tentativeG = gScore[current] + GetEdgeWeight(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    else
                        openSet.UpdatePriority(neighbor, fScore[neighbor]);
                }
            }
        }

        return null;
    }

    public static Dictionary<VolumeAgent, float> Dijkstra(VolumeAgent origin)
    {
        var openSet = new SimplePriorityQueue<VolumeAgent>();
        var gScore = new Dictionary<VolumeAgent, float>();
        var visited = new HashSet<VolumeAgent>();

        openSet.Enqueue(origin, 0);
        gScore[origin] = 0;

        while (openSet.Count > 0)
        {
            VolumeAgent current = openSet.Dequeue();
            visited.Add(current);

            List<VolumeAgent> neighbors = VolumeGridBuilder.graph.GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                float tentativeG = gScore[current] + GetEdgeWeight(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeG;

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, tentativeG);
                    else
                        openSet.UpdatePriority(neighbor, tentativeG);
                }
            }
        }

        return gScore;
    }

    public static Dictionary<VolumeAgent, int> BreadthFirstSearch(VolumeAgent start)
    {
        var distances = new Dictionary<VolumeAgent, int>();
        var queue = new Queue<VolumeAgent>();

        distances[start] = 0;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int currentDist = distances[current];

            List<VolumeAgent> neighbors = VolumeGridBuilder.graph.GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                if (!distances.ContainsKey(neighbor))
                {
                    distances[neighbor] = currentDist + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return distances;
    }

    public static List<VolumeAgent> DFS(VolumeAgent start, VolumeAgent goal)
    {
        var stack = new Stack<VolumeAgent>();
        var cameFrom = new Dictionary<VolumeAgent, VolumeAgent>();
        var visited = new HashSet<VolumeAgent>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            VolumeAgent current = stack.Pop();
            if (current == goal)
                return ReconstructPath(cameFrom, current);

            List<VolumeAgent> neighbors = VolumeGridBuilder.graph.GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    stack.Push(neighbor);
                }
            }
        }

        return null;
    }

    private static float Heuristic(VolumeAgent a, VolumeAgent b)
    {
        return Vector3.Distance(a.worldPosition, b.worldPosition);
    }

    private static float GetEdgeWeight(VolumeAgent from, VolumeAgent to)
    {
        foreach (var edge in from.edges)
        {
            if (edge.to == to)
                return edge.weight;
        }
        return float.MaxValue;
    }

    private static List<VolumeAgent> ReconstructPath(Dictionary<VolumeAgent, VolumeAgent> cameFrom, VolumeAgent current)
    {
        var totalPath = new List<VolumeAgent> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}
