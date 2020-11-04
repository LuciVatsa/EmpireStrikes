using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(door.GetComponent<DoorScript>().getDoorStatus())
        {
            float xDist = Mathf.Abs(transform.position.x - player.transform.position.x);
            float yDist = Mathf.Abs(transform.position.y - player.transform.position.y);
            float zDist = Mathf.Abs(transform.position.z - player.transform.position.z);
            if (xDist < 5 && yDist < 5 && zDist < 5)
            {
                player.transform.position = new Vector3(71, 35, -23);
            }
        }
    }


    

}
