using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class DroneAgentStatus : MonoBehaviour
{
    [Header("Floating Label Settings")]
    public float labelHeight = 2f;
    public float labelScale = 0.1f;
    public Color textColor = Color.white;

    private TextMeshPro textLabel;
    private bool initialized = false;

    // Flight info
    private string flightID;
    private string callSign;
    private List<VolumeAgent> path;
    private VolumeAgent currentAgent;
    private VolumeAgent originAgent;
    private VolumeAgent destinationAgent;

    public void SetFlightInfo(string id, List<VolumeAgent> path, VolumeAgent origin, VolumeAgent destination, string callSign)
    {
        this.flightID = id;
        this.path = path;
        this.originAgent = origin;
        this.destinationAgent = destination;
        this.callSign = callSign;
        initialized = true;
    }

    void Start()
    {
        Transform root = GameObject.Find("DroneStatusLabelRoot")?.transform;
        if (root == null)
        {
            GameObject rootObj = new GameObject("DroneStatusLabelRoot");
            root = rootObj.transform;
            root.localPosition += new Vector3(0, 15, 0);
        }

        GameObject labelGO = new GameObject("DroneStatusLabel");
        labelGO.transform.SetParent(root);
        labelGO.transform.position = transform.position + Vector3.up * labelHeight + new Vector3(0, 8f, 0);
        labelGO.transform.localScale = Vector3.one * labelScale;

        textLabel = labelGO.AddComponent<TextMeshPro>();
        textLabel.fontSize = 64f;
        textLabel.color = textColor;
        textLabel.alignment = TextAlignmentOptions.Center;
        textLabel.textWrappingMode = TextWrappingModes.NoWrap;
        textLabel.text = "(initializing)";
    }

    void Update()
    {
        if (!initialized || textLabel == null) return;

        textLabel.transform.position = transform.position + Vector3.up * labelHeight + new Vector3(0, 8f, 0);

        if (path != null && path.Count > 0)
        {
            int tEstimate = Mathf.RoundToInt(Time.time / VolumeGridBuilder.graph.volSize * VolumeGridBuilder.GlobalDroneSpeed);
            currentAgent = VolumeUtils.FindClosestAgent(transform.position, tEstimate);
        }

        UpdateText();
    }

    void LateUpdate()
    {
        if (textLabel != null && Camera.main != null)
        {
            textLabel.transform.rotation = Quaternion.LookRotation(
                textLabel.transform.position - Camera.main.transform.position);
        }
    }

    void UpdateText()
    {
        string current = currentAgent != null
            ? $"{currentAgent.gridPosition}  t={currentAgent.timeIndex}"
            : "Unknown";

        string origin = originAgent != null ? $"{originAgent.gridPosition}" : "Unset";
        string dest   = destinationAgent != null ? $"{destinationAgent.gridPosition}" : "Unset";

        float altitude = transform.position.y;
        string altitudeStr = $"{altitude:F1}m";

        textLabel.text =
            $"Flight: {callSign}\n" +
            $"Alt: {altitudeStr}\n" +
            $"Pos: {current}\n" +
            $"From: {origin}\n" +
            $"To:   {dest}";
    }
} 
