using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DoorScript : MonoBehaviour
{
    private bool isOpen;
    private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }
    public bool getDoorStatus()
    {
        return isOpen;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnPress()
    {
        if (!isOpen)
        {
            transform.position += new Vector3(0, -5, 0);
            isOpen = true;
        }
        else
        {
            transform.position += new Vector3(0, 5, 0);
            isOpen = false;
        }
    }
    public void open()
    {
        if (!isOpen)
        {
            transform.position += new Vector3(0, -5, 0);
            isOpen = true;
        }
    }

    public void close()
    {
        if (isOpen)
        {
            transform.position += new Vector3(0, 5, 0);
            isOpen = false;
        }
    }
}
