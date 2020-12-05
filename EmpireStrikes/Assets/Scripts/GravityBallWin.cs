using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityBallWin : MonoBehaviour {
    public Text negativeXText;
    public Text positiveXText;
    public Text negativeZText;
    public Text positiveZText;

    // Override
    public void Start() {
        this.negativeXText.enabled = false;
        this.positiveXText.enabled = false;
        this.negativeZText.enabled = false;
        this.positiveZText.enabled = false;
    }

    // Override
    private void OnTriggerEnter(Collider other) {
        Debug.Log("WIN");
        this.negativeXText.enabled = true;
        this.positiveXText.enabled = true;
        this.negativeZText.enabled = true;
        this.positiveZText.enabled = true;
    }
}

