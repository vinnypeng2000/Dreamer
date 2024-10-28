using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the object around the z-axis at the specified speed
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}