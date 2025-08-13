using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    // Speed of the skybox rotation
    public float rotationSpeed = 1.0f;

    void Update()
    {
        // Rotate the skybox by modifying its "_Rotation" property
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}