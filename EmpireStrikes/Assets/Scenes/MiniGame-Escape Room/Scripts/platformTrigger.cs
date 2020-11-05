using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformTrigger : MonoBehaviour
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
       // if (triggeringObject.transform.position )
    }

    public void checkDistance()
    {
        float xDist = Mathf.Abs(transform.position.x - triggeringObject.transform.position.x);
        float yDist = Mathf.Abs(transform.position.y - triggeringObject.transform.position.y);
        float zDist = Mathf.Abs(transform.position.z - triggeringObject.transform.position.z);
        if (xDist < 5 && yDist < 5 && zDist < 5)
        {
            door.GetComponent<DoorScript>().open();
        }
    }

}
