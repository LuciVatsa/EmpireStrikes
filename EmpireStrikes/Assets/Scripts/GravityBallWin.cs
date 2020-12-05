using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallWin : MonoBehaviour {
    // Override
    private void OnTriggerEnter(Collider other) {
        Debug.Log("WIN");
    }
}

