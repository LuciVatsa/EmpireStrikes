using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRoom : MonoBehaviour {
    // Override
    void Update() {
        float rotateX = 0f;
        float rotateZ = 0f;
        float rotateSpeed = 1f;

        if (Input.GetKey(KeyCode.W)) {
            rotateX -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.S)) {
            rotateX += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.A)) {
            rotateZ -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.D)) {
            rotateZ += rotateSpeed;
        }

        transform.Rotate(Vector3.right, rotateX);
        transform.Rotate(Vector3.forward, rotateZ);
    }
}

