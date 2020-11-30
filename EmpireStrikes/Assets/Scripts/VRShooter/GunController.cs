using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject gunGameObject;
    public FireBullets fireBulletsScript;

    void Update()
    {
        this.transform.position = gunGameObject.transform.position;
        this.transform.rotation = gunGameObject.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Mag"))
        //{
        //    if (!other.GetComponent<Reload>().GrabLastFrameForReload)
        //    {
        //        other.gameObject.transform.parent = this.gameObject.transform;
        //        //if (!other.GetComponent<Reload>().HasAttached)
        //        //{
        //        //    fireBulletsScript.Reload(other.GetComponent<Reload>().BulletCount);
        //        //    other.GetComponent<Reload>().HasAttached = true;
        //        //}

        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mag"))
        {
            if (!other.GetComponent<Reload>().GrabLastFrameForReload)
            {
                other.gameObject.transform.parent = null;
            }
        }
    }
}
