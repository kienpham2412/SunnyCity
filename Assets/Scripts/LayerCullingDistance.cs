using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCullingDistance : MonoBehaviour
{
    public Camera cam;
    public float[] layers;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        cam.layerCullDistances = layers;
    }
}
