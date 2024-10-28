using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{

    public GameObject readText;
    public GameObject bookUI;
    public bool read;
    public GameObject exitText;

    // Start is called before the first frame update
    void Start()
    {   
        readText.SetActive(false);
        bookUI.SetActive(false);
        read = false;
        exitText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (read)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bookUI.SetActive(true);
                exitText.SetActive(true);
                readText.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bookUI.SetActive(false);
                read = false;
                exitText.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readText.SetActive(true);
            read = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readText.SetActive(false);
            read = false;
        }
    }
}
