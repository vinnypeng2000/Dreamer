using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightController : MonoBehaviour
{
    public Light spotlight;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI exitText;
    public GameObject player;
    public MonoBehaviour playerMovementScript;
    public float burnSpeed = 0.2f;
    public float raycastDistance = 50f;
    public Camera cam;
    public Vector3 targetRotation;  // Set in Inspector
    public Vector3 targetPosition;  // Set in Inspector, no default

    private bool isPlayerInRange = false;
    private bool isControllingLight = false;

    private List<string> burnSequence = new List<string> { "Money", "Laptop", "Sandclock" };
    private int currentBurnIndex = 0;

    void Start()
    {
        actionText.gameObject.SetActive(false);
        exitText.gameObject.SetActive(false);
        // Debug.Log(targetPosition);
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
            actionText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            actionText.gameObject.SetActive(false);
        }
    }

    void EnterOperationMode()
    {
        isControllingLight = true;
        Debug.Log("Script attached to: " + gameObject.name);
        Debug.Log("Target Position: " + targetPosition);
        player.transform.position = targetPosition; // Use Inspector value for targetPosition
        cam.transform.rotation = Quaternion.Euler(targetRotation);
        actionText.gameObject.SetActive(false);
        exitText.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerMovementScript.enabled = false;
    }

    void ExitOperationMode()
    {
        isControllingLight = false;
        exitText.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerMovementScript.enabled = true;
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
        Ray ray = new Ray(spotlight.transform.position, spotlight.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(spotlight.transform.position, spotlight.transform.forward * raycastDistance, Color.red);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Burnable") && currentBurnIndex < burnSequence.Count)
            {
                string objectName = hit.collider.gameObject.name;

                if (objectName == burnSequence[currentBurnIndex])
                {
                    StartCoroutine(BurnObject(hit.collider.gameObject));
                    currentBurnIndex++;

                    if (currentBurnIndex >= burnSequence.Count)
                    {
                        Debug.Log("All objects burned in the correct order!");
                    }
                }
            }
        }
    }

    System.Collections.IEnumerator BurnObject(GameObject burnable)
    {
        if (burnable == null) yield break;

        Renderer renderer = burnable.GetComponent<Renderer>();
        if (renderer == null) yield break;

        float burnAmount = 0f;
        Vector3 originalScale = burnable.transform.localScale;

        while (burnAmount < 1f)
        {
            burnAmount += Time.deltaTime * burnSpeed;

            if (renderer == null) yield break;
            if (renderer.material.HasProperty("_Color"))
            {
                Color color = renderer.material.color;
                color.a = Mathf.Lerp(1f, 0f, burnAmount);
                renderer.material.color = color;
            }

            burnable.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, burnAmount);
            yield return null;
        }

        Destroy(burnable);
    }
}
