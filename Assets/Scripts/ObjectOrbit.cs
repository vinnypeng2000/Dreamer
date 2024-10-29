using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOrbit : MonoBehaviour
{
    public Transform centralObject; // The object at the center of the orbit
    public float orbitRadius = 3f;  // Radius of the orbit
    public float orbitSpeed = 50f;  // Speed of the orbit

    private float currentAngle;     // Current angle of orbit

    void Start()
    {
        // Calculate the initial angle based on the object's position relative to the central object
        Vector3 offset = transform.position - centralObject.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x);
    }

    void Update()
    {
        // Update the current angle based on the orbit speed
        currentAngle += orbitSpeed * Time.deltaTime;

        // Calculate the new position based on the updated angle
        float x = centralObject.position.x + Mathf.Cos(currentAngle) * orbitRadius;
        float z = centralObject.position.z + Mathf.Sin(currentAngle) * orbitRadius;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}

