using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    public void Fire()
    {
        GameObject shot = GameObject.Instantiate(bullet, transform.position, transform.rotation);
        shot.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*10000);
        Destroy(shot,2);
        //shot.gameObject.AddComponent<Rigidbody>().AddForce(transform.forward*10000);

    }

    private void Update()
    {
        Vector3 forward = transform.forward * 10;
        Debug.DrawRay(transform.position,forward,Color.red);
    }
}
