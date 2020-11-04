using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class fireTrigger : MonoBehaviour
{
    public GameObject triggeringObject;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggeringObject.GetComponent<FireSource>().isBurning)
        {
            door.GetComponent<DoorScript>().open();
        }
    }

   

}
