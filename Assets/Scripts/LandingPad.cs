using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LandingPad : MonoBehaviour
{
    public string padID;
    public bool isOccupied = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            isOccupied = true;
            Debug.Log($"Drone landed on pad {padID}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            isOccupied = false;
            Debug.Log($"Drone took off from pad {padID}");
        }
    }
}
