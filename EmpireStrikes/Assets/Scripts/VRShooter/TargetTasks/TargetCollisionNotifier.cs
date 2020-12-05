using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollisionNotifier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Bullet))
        {

        }
    }
}
