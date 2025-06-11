using UnityEngine;
using System.Collections.Generic;

public class ATCController : MonoBehaviour
{
    [Header("Pad Settings")]
    public Transform[] pads;
    private bool[] padOccupied;

    private void Awake()
    {
        padOccupied = new bool[pads.Length];
    }

    public bool HasAvailablePad()
    {
        foreach (bool occupied in padOccupied)
        {
            if (!occupied) return true;
        }
        return false;
    }

    public Vector3 GetSpawnPoint()
    {
        for (int i = 0; i < pads.Length; i++)
        {
            if (!padOccupied[i])
            {
                padOccupied[i] = true;
                return pads[i].position;
            }
        }
        Debug.LogWarning("No available pads! Returning first pad.");
        return pads[0].position;
    }

    public void NotifyTakeoff(DroneAgent agent)
    {
        for (int i = 0; i < pads.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, pads[i].position) < 1.5f)
            {
                padOccupied[i] = false;
                break;
            }
        }
    }

    public void NotifyLanding(DroneAgent agent)
    {
        for (int i = 0; i < pads.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, pads[i].position) < 1.5f)
            {
                padOccupied[i] = true;
                break;
            }
        }
    }
}