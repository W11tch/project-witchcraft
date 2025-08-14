using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectWitchcraft.Core // A more general namespace
{
    public class VisualsController : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private Material _originalMaterial;
        private Material _transparentMaterialInstance;

        // Cache the shader property IDs for better performance
        private static readonly int _Surface = Shader.PropertyToID("_Surface");
        private static readonly int _ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int _SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int _DstBlend = Shader.PropertyToID("_DstBlend");

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null)
            {
                // Store the original material to revert back to
                _originalMaterial = _meshRenderer.sharedMaterial;
                // Create a single instance of the transparent material to reuse
                _transparentMaterialInstance = new Material(_originalMaterial);
            }
        }

        /// <summary>
        /// Sets the material to either transparent or opaque with a specified alpha value.
        /// </summary>
        /// <param name="isTransparent">If true, the material will be set to transparent. If false, it will be opaque.</param>
        /// <param name="alpha">The alpha value to use when setting to transparent.</param>
        public void SetIsTransparent(bool isTransparent, float alpha = 1.0f)
        {
            if (_meshRenderer == null) return;

            if (isTransparent)
            {
                // Use our dedicated transparent material instance
                _meshRenderer.material = _transparentMaterialInstance;

                // Set material to transparent state
                _transparentMaterialInstance.SetOverrideTag("RenderType", "Transparent");
                _transparentMaterialInstance.SetInt(_Surface, 1); // 1 = Transparent
                _transparentMaterialInstance.SetInt(_ZWrite, 0);   // Don't write to depth buffer
                _transparentMaterialInstance.SetInt(_SrcBlend, (int)BlendMode.SrcAlpha);
                _transparentMaterialInstance.SetInt(_DstBlend, (int)BlendMode.OneMinusSrcAlpha);
                _transparentMaterialInstance.renderQueue = (int)RenderQueue.Transparent;

                // Apply the new alpha value
                Color color = _transparentMaterialInstance.color;
                color.a = alpha;
                _transparentMaterialInstance.color = color;
            }
            else
            {
                // Revert to the original, shared material
                _meshRenderer.material = _originalMaterial;
            }
        }
    }
}