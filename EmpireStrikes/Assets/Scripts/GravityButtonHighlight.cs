using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButtonHighlight : MonoBehaviour {
    public Camera playerCamera;

    public MeshRenderer frontButton;
    public MeshRenderer backButton;
    public MeshRenderer leftButton;
    public MeshRenderer rightButton;

    public Material highlightMaterial;

    private Material defaultMaterial;

    // Override
    void Start() {
        this.defaultMaterial = this.frontButton.material;
    }

    // Update is called once per frame
    void Update() {
        this.frontButton.material = this.defaultMaterial;
        this.backButton.material = this.defaultMaterial;
        this.leftButton.material = this.defaultMaterial;
        this.rightButton.material = this.defaultMaterial;

        float angleX = (
            this.playerCamera.transform.eulerAngles.x
        );
        float angleY = (
            this.playerCamera.transform.eulerAngles.y
        );
Debug.Log(angleY);
        if (angleX >= 13.5f && angleX <= 20f) {
            if (angleY >= -2.5f && angleY <= 2.5f) {
                this.frontButton.material = this.highlightMaterial;
            }
            else if (angleY >= (90f - 2.5f) && angleY <= (90f + 2.5f)) {
                this.rightButton.material = this.highlightMaterial;
            }
            else if (angleY >= (180f - 2.5f) && angleY <= (180f + 2.5f)) {
                this.backButton.material = this.highlightMaterial;
            }
            else if (angleY >= (270f - 2.5f) && angleY <= (270f + 2.5f)) {
                this.leftButton.material = this.highlightMaterial;
            }
        }
    }
}

