using UnityEngine;
using System.Collections.Generic;

public class DroneAgent : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    private List<Vector3> path;
    private int currentWaypointIndex = 0;
    private bool isInitialized = false;

    public void InitializePath(List<Vector3> waypoints)
    {
        path = new List<Vector3>(waypoints);
        currentWaypointIndex = 0;
        isInitialized = true;
    }



    void Update()
    {
        if (!isInitialized || path == null || currentWaypointIndex >= path.Count)
            return;

        Vector3 target = path[currentWaypointIndex];
        Vector3 moveDir = (target - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target);
        if (distance < 0.5f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= path.Count)
            {
                OnArrival();
            }
        }
    }

    private void OnArrival()
    {
        Debug.Log(name + " reached destination.");
        // You can expand this to notify ATC or UNetController
        Destroy(gameObject, 2f);
    }

    public bool HasReachedDestination()
    {
        return isInitialized && currentWaypointIndex >= path.Count;
    }

    public Vector3 GetCurrentTarget()
    {
        if (!isInitialized || path == null || currentWaypointIndex >= path.Count)
            return transform.position;

        return path[currentWaypointIndex];
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public List<Vector3> GetFullPath()
    {
        return path;
    }
} 
