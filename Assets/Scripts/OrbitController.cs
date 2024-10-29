using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float rotationSpeed = 50f; // Speed of the orbit
    public List<GameObject> orbitingObjects = new List<GameObject>(); // List of objects to orbit around the center

    void Update()
    {
        // Rotate the central pivot to make the objects orbit
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Optional: Adjust the distance of orbiting objects from the pivot
        for (int i = 0; i < orbitingObjects.Count; i++)
        {
            Vector3 direction = (orbitingObjects[i].transform.position - transform.position).normalized;
            orbitingObjects[i].transform.position = transform.position + direction * 3f; // Adjust distance (3f here)
        }
    }
}
