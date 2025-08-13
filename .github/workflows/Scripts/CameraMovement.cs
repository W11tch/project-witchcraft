using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // The player model's transform to follow.
    // Make sure to assign the child GameObject with the visual model here.
    public Transform playerModel;

    // The offset from the player's position
    public Vector3 offset;

    // Controls the time it takes for the camera to reach the target.
    // Smaller values mean a faster, less-smooth transition.
    [Tooltip("The time in seconds it takes for the camera to reach the target.")]
    public float smoothTime = 0.25f;

    // Use LateUpdate for camera movement to ensure it happens after all player movement
    // This prevents the camera from "jittering" or lagging behind the player
    void LateUpdate()
    {
        // Check if the player transform is assigned to avoid errors
        if (playerModel != null)
        {
            // Calculate the desired position of the camera based on the player model's position
            Vector3 desiredPosition = playerModel.position + offset;

            // Smoothly move the camera from its current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime * Time.deltaTime);

            // Set the camera's final position
            transform.position = smoothedPosition;
        }
    }
}
