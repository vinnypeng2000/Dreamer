using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed; // Speed of rotation in degrees per second
    public LightController light;

    private bool shouldSpin = true;
    private BoxCollider boxCollider;
    private Renderer[] childRenderers;
    public GameObject inspectText;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        childRenderers = GetComponentsInChildren<Renderer>();

        // Set the collider to inactive while spinning
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        // Set initial glow effect off
        SetGlowEffect(false);
    }

    void Update()
    {
        if (this.gameObject.CompareTag("Car") && shouldSpin)
        {
            // Find the matching burnable object by name in the burn sequence
            for (int i = 0; i < light.burnSequence.Count; i++)
            {
                if (this.name == light.burnSequence[i].name)
                {
                    // Stop spinning if the corresponding orb is burned
                    if (light.burnSequence[i].done)
                    {
                        shouldSpin = false;
                        ToggleCarState(false);
                        break;
                    }
                }
            }

            if (shouldSpin)
            {
                transform.Rotate(0, spinSpeed * Time.deltaTime, 0); // Spin only if shouldSpin is true
            }
        }
        else if (!this.gameObject.CompareTag("Car"))
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
    }

    private void ToggleCarState(bool isSpinning)
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = !isSpinning;
        }
        SetGlowEffect(!isSpinning); // Apply glow effect when car stops spinning
    }

    private void SetGlowEffect(bool isActive)
    {
        Color glowColor = new Color(1f, 0.8f, 0f) * 0.3f; // Adjust RGB and intensity as needed

        foreach (Renderer renderer in childRenderers)
        {
            if (isActive)
            {
                // Enable emission with a subtle glow color
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", glowColor);
            }
            else
            {
                // Disable emission to remove glow effect
                renderer.material.DisableKeyword("_EMISSION");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inspectText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inspectText.SetActive(false);
        }
    }
}