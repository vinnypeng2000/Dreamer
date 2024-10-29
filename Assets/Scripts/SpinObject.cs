using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f; // Speed of rotation in degrees per second

    void Update()
    {
        if (this.gameObject.CompareTag("Car"))
        {
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }
        else
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}