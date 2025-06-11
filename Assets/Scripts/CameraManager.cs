using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public KeyCode nextKey = KeyCode.C;
    public KeyCode overviewKey = KeyCode.V;
    public Camera overviewCamera;

    private List<Camera> droneCams = new();
    private int currentIndex = -1;

    void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            CycleToNextCamera();
        }

        if (Input.GetKeyDown(overviewKey) && overviewCamera != null)
        {
            ActivateOnly(overviewCamera);
        }
    }

    public void RegisterDroneCamera(Camera cam)
    {
        cam.enabled = false;
        droneCams.Add(cam);

        if (droneCams.Count == 1)
        {
            currentIndex = 0;
            cam.enabled = true;
        }
    }

    void CycleToNextCamera()
    {
        if (droneCams.Count == 0) return;

        currentIndex = (currentIndex + 1) % droneCams.Count;
        ActivateOnly(droneCams[currentIndex]);
    }

    void ActivateOnly(Camera targetCam)
    {
        foreach (var cam in droneCams) cam.enabled = false;
        if (overviewCamera != null) overviewCamera.enabled = false;

        targetCam.enabled = true;
    }
}
