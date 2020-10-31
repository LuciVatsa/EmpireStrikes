using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRotateFront : GravityButtonTrigger {
    public override void OnPress() {
        transform.Rotate(Vector3.right, -30f * Time.deltaTime, Space.World);
    }
}
