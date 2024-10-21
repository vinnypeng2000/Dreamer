using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Speed of movement
    public float jumpForce; // Force of jump
    private Rigidbody rb;

    public float mouseSensitivity = 2f; // Sensitivity for mouse movement
    public Transform cameraTransform; // Reference to the camera transform
    private float verticalRotation = 0f; // Track vertical rotation

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of the screen
    }

    private void Update()
    {
        Move(); // Call the Move function
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(); // Call the Jump function if the Space key is pressed
        }
        RotateCamera();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right Arrow)
        float moveVertical = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down Arrow)

        // Create a movement vector based on the camera's forward direction
        Vector3 forward = cameraTransform.forward; // Get the camera's forward direction
        forward.y = 0; // Ignore vertical component
        forward.Normalize(); // Normalize to get a direction vector

        Vector3 right = cameraTransform.right; // Get the camera's right direction
        right.y = 0; // Ignore vertical component
        right.Normalize(); // Normalize to get a direction vector

        Vector3 movement = (right * moveHorizontal + forward * moveVertical); // Combine movement vectors

        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime); // Move the player
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.01f) // Check if player is on the ground
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply jump force
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // Get horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // Get vertical mouse movement

        verticalRotation -= mouseY; // Update vertical rotation
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 30f); // Clamp vertical rotation to avoid flipping

        // Rotate the camera and player
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f); // Rotate camera up and down
        transform.Rotate(Vector3.up * mouseX); // Rotate player left and right
    }
}
