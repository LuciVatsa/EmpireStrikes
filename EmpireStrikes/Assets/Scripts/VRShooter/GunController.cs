using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject magGameObject;
    public GameObject magPosition;
    public Reload reloadScript;
    void Update()
    {
        if(!reloadScript.IsReloading)
            magGameObject.transform.position = magPosition.transform.position;
    }
}
