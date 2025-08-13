using UnityEngine;
using UnityEngine.Rendering;

public class VisualsController : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    // Cache the shader property IDs for better performance
    private static readonly int _Surface = Shader.PropertyToID("_Surface");
    private static readonly int _ZWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int _SrcBlend = Shader.PropertyToID("_SrcBlend");
    private static readonly int _DstBlend = Shader.PropertyToID("_DstBlend");

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // Ensure we always have a unique material instance to work with
            meshRenderer.material = new Material(meshRenderer.sharedMaterial);
        }
    }

    /// <summary>
    /// Sets the material to either transparent or opaque with a specified alpha value.
    /// </summary>
    /// <param name="isTransparent">If true, the material will be set to transparent. If false, it will be opaque.</param>
    /// <param name="alpha">The alpha value to use when setting to transparent.</param>
    public void SetIsTransparent(bool isTransparent, float alpha = 1.0f)
    {
        if (meshRenderer == null || meshRenderer.material == null) return;

        if (isTransparent)
        {
            // Set material to transparent state using cached IDs and enums
            meshRenderer.material.SetOverrideTag("RenderType", "Transparent");
            meshRenderer.material.SetInt(_Surface, 1);
            meshRenderer.material.SetInt(_ZWrite, 0);
            meshRenderer.material.SetInt(_SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meshRenderer.material.SetInt(_DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            // Apply the new alpha value
            Color color = meshRenderer.material.color;
            color.a = alpha;
            meshRenderer.material.color = color;
        }
        else
        {
            // Set material to opaque state using cached IDs
            meshRenderer.material.SetOverrideTag("RenderType", "");
            meshRenderer.material.SetInt(_Surface, 0);
            meshRenderer.material.SetInt(_ZWrite, 1);
            meshRenderer.material.SetInt(_SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
            meshRenderer.material.SetInt(_DstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
            meshRenderer.material.renderQueue = -1;

            // Ensure alpha is fully opaque
            Color color = meshRenderer.material.color;
            color.a = 1.0f;
            meshRenderer.material.color = color;
        }
    }
}