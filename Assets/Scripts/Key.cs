using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public DoorController doorController;
    public GameObject text;
    public bool entered;

    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && entered)
        {
            doorController.key = true;
            text.SetActive(false);
            Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
            entered = true;
        }
    }
}
