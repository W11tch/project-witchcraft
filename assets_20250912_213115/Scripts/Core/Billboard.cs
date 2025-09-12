// Located at: Assets/Scripts/Core/Billboard.cs
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    /// <summary>
    /// This component makes the GameObject it's attached to always face the main camera.
    /// For a top-down view, it works by matching the camera's X-axis rotation.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null) return;

            // **THE FIX:**
            // To make the sprite tilt up towards a top-down camera, we get the camera's rotation,
            // but we only apply the rotation from its X-axis. We keep the object's current Y and Z rotation.
            transform.rotation = Quaternion.Euler(
                _cameraTransform.rotation.eulerAngles.x, 0, 0 
             );
        }
    }
}