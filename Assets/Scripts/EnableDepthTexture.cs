using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableDepthTexture : MonoBehaviour
{
    void Start()
    {
        print("Enabling");

        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        print(DepthTextureMode.Depth);
    }
}
