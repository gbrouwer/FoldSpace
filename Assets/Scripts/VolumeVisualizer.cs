using System.Collections.Generic;
using UnityEngine;

public static class VolumeVisualizer
{
    private static GameObject visualPrefab;
    private static Transform root;

    public static void Initialize(GameObject prefab, string rootName = "VolumeVisuals")
    {
        visualPrefab = prefab;
        root = GameObject.Find(rootName)?.transform;

        if (root == null)
        {
            GameObject rootObj = new GameObject(rootName);
            root = rootObj.transform;
        }
    }

    public static void ShowVolumes(List<VolumeAgent> path)
    {
        foreach (var volume in path)
        {
            GameObject vis = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vis.transform.position = volume.worldPosition;
            vis.transform.localScale = Vector3.one * 1.0f;
            vis.name = $"VolumeVis_{volume.gridPosition}_{volume.timeIndex}";
            vis.transform.parent = root;
            Color c = Color.Lerp(Color.blue, Color.red, 1);



            var renderer = vis.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Clone the material to ensure per-volume instance
                Material matInstance = new Material(renderer.sharedMaterial);

                // Set base and emission colors
                matInstance.SetColor("_BaseColor", c);
                matInstance.SetColor("_EmissionColor", c * 2f); // Optional: boost intensity

                // Ensure emission is actually turned on
                matInstance.EnableKeyword("_EMISSION");

                // Apply to the volume
                renderer.material = matInstance;
            }

        }
    }

    public static void ClearVisuals()
    {
        if (root == null) return;
        foreach (Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
} 
