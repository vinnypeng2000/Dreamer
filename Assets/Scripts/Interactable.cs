using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Color glowColor = new Color(1f, 0.8f, 0f); // Customize color if needed
    [SerializeField] private float glowIntensity = 0.3f; // Adjust intensity as needed for subtle glow

    private Renderer objectRenderer;

    void Start()
    {
        // Get all child renderers to apply the glow effect
        objectRenderer = GetComponent<Renderer>();
        ApplyGlowEffect(true); // Enable glow on start
    }

    private void ApplyGlowEffect(bool isActive)
    {
        Color finalGlowColor = glowColor * glowIntensity;

        if (objectRenderer != null)
        {
            if (isActive)
            {
                // Enable emission with a subtle glow color
                objectRenderer.material.EnableKeyword("_EMISSION");
                objectRenderer.material.SetColor("_EmissionColor", finalGlowColor);
            }
            else
            {
                // Disable emission to remove glow effect
                objectRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
}
