using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraEmulator : MonoBehaviour {
    private Camera playerCamera;

    // Override
    void Start() {
        this.playerCamera = this.GetComponentInChildren<Camera>();
    }

    #if UNITY_EDITOR
    // Override
    void Update() {
        if (this.isTurnModeInput()) {
            this.playerCamera.transform.Rotate(
                this.getVerticalInput(),
                this.getHorizontalInput(),
                -this.playerCamera.transform.eulerAngles.z
            );
        }
        else if (this.isTiltModeInput()) {
            this.playerCamera.transform.Rotate(
                0f,
                0f,
                this.getHorizontalInput()
            );
        }
    }
    #endif

    private bool isTurnModeInput() {
        return (
            Input.GetKey(KeyCode.RightAlt) ||
            Input.GetKey(KeyCode.LeftAlt)
        );
    }

    private bool isTiltModeInput() {
        return (
            Input.GetKey(KeyCode.RightControl) ||
            Input.GetKey(KeyCode.LeftControl)
        );
    }

    private float getHorizontalInput() {
        return (
            4f * Input.GetAxis("Mouse X")
        );
    }

    private float getVerticalInput() {
        return (
            -4f * Input.GetAxis("Mouse Y")
        );
    }
}
