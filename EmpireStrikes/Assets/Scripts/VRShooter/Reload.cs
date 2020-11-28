using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Reload : MonoBehaviour
{
    public SteamVR_Action_Boolean GunReload;

    

    public SteamVR_Input_Sources handTypeForReload;


    public bool GrabLastFrameForReload = false;

    private Hand RefToHandReload = null;

    public bool HasAttached = false;
    public int BulletCount;

    private void Start()
    {
        BulletCount = 30;
        GunReload.AddOnStateUpListener(ReloadHold, handTypeForReload);
        GunReload.AddOnStateDownListener(ReloadRelease, handTypeForReload);
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
       
        if (GrabLastFrameForReload)
        {
            RefToHandReload = hand;
            GrabLastFrameForReload = false;
            hand.AttachObject(this.gameObject, GrabTypes.Scripted);
        }


    }


    private void ReloadRelease(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        
        
            GrabLastFrameForReload = false;
           

    }

    private void ReloadHold(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {

        if (RefToHandReload )
        {
            RefToHandReload.DetachObject(this.gameObject);
            RefToHandReload = null;
        }
        else
        {
       
            GrabLastFrameForReload = true;
        }
    }
}
