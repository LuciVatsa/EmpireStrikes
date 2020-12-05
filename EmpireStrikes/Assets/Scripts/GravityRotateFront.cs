using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRotateFront : GravityButtonTrigger {
    public GameObject room;

    public override void OnPress() {
        float rotation = -30f * Time.deltaTime;
        room.transform.Rotate(Vector3.right, rotation, Space.World);
    }
}

