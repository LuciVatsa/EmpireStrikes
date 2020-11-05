using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButtonPress : MonoBehaviour {
    public Camera playerCamera;
    public GravityButtonTrigger buttonPressScript;

    // Override
    void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 cameraPosition = this.playerCamera.transform.position;
            Vector3 cameraDirection = this.playerCamera.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(cameraPosition, cameraDirection, out hit)) {
                if (hit.transform.gameObject == this.gameObject) {
                    this.buttonPressScript.OnPress();
                }
            }
        }
    }
}
