using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightController : MonoBehaviour
{
    public Light spotlight; // Reference to the spotlight
    public TextMeshProUGUI actionText; // UI Text for "Press E to Operate"
    public TextMeshProUGUI exitText; // UI Text for "Press ESC to Exit"
    public GameObject player; // Reference to the player object
    public MonoBehaviour playerMovementScript; // Reference to the player movement script
    public float burnSpeed = 0.2f; // Speed at which objects burn away
    public float raycastDistance = 50f; // Maximum distance for the raycast
    public Camera cam;
    public Vector3 targetRotation;
    public Vector3 targetPosition = new Vector3(-10.44f,0.82f,2.7f);

    private bool isPlayerInRange = false; // To check if the player is near the trigger
    private bool isControllingLight = false; // To check if the player is controlling the light

    void Start()
    {
        actionText.gameObject.SetActive(false); // Hide "Press E to Operate" text initially
        exitText.gameObject.SetActive(false); // Hide "Press ESC to Exit" text initially
    }

    void Update()
    {
        if (isPlayerInRange && !isControllingLight)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                EnterOperationMode();
            }
        }

        if (isControllingLight)
        {
            ControlLightDirection();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitOperationMode();
            }

            RaycastAndBurn();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
            actionText.gameObject.SetActive(true); // Show "Press E to Operate" text when player enters the trigger
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            actionText.gameObject.SetActive(false); // Hide the text when player leaves the trigger
        }
    }

    void EnterOperationMode()
    {
        isControllingLight = true;
        player.transform.position = targetPosition;
        cam.transform.rotation = Quaternion.Euler(targetRotation);
        actionText.gameObject.SetActive(false); // Hide the "Press E to Operate" text
        exitText.gameObject.SetActive(true); // Show the "Press ESC to Exit" text
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor for light control
        playerMovementScript.enabled = false; // Disable player movement
    }

    void ExitOperationMode()
    {
        isControllingLight = false;
        exitText.gameObject.SetActive(false); // Hide the "Press ESC to Exit" text
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
        playerMovementScript.enabled = true; // Enable player movement
    }

    void ControlLightDirection()
    {

        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        spotlight.transform.Rotate(Vector3.up, mouseX, Space.World);
        spotlight.transform.Rotate(Vector3.right, -mouseY, Space.Self);
    }

    void RaycastAndBurn()
    {
        // Cast a ray from the spotlight to detect objects in the spotlight's direction
        Ray ray = new Ray(spotlight.transform.position, spotlight.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(spotlight.transform.position, spotlight.transform.forward * raycastDistance, Color.red);


        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object is burnable
            if (hit.collider.CompareTag("Burnable"))
            {
                // Start the burn effect on the object
                StartCoroutine(BurnObject(hit.collider.gameObject));
            }
        }
    }

    // Coroutine to burn the object by reducing its scale over time
    System.Collections.IEnumerator BurnObject(GameObject burnable)
    {
        Renderer renderer = burnable.GetComponent<Renderer>();
        if (renderer == null) yield break;

        float burnAmount = 0f;
        Vector3 originalScale = burnable.transform.localScale;

        // Gradually burn the object by scaling it down over time
        while (burnAmount < 1f)
        {
            burnAmount += Time.deltaTime * burnSpeed;

            // Optional: Apply material transparency or color change to simulate burning
            if (renderer.material.HasProperty("_Color"))
            {
                Color color = renderer.material.color;
                color.a = Mathf.Lerp(1f, 0f, burnAmount); // Gradually make the object transparent
                renderer.material.color = color;
            }

            // Scale down the object gradually to simulate it burning away
            burnable.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, burnAmount);

            // Wait for the next frame
            yield return null;
        }

        // Destroy the object after it's fully burned away
        Destroy(burnable);
    }
}

