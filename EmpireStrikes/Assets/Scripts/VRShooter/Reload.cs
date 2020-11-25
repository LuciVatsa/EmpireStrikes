using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Reload : MonoBehaviour
{
    public SteamVR_Action_Boolean GunReload;

    [SerializeField] private GameObject reloadGameObject;

    public SteamVR_Input_Sources handTypeForReload;


    private bool GrabLastFrameForReload = false;

    private Hand RefToHandReload = null;

    public bool IsReloading = false;


    private void Start()
    {
        GunReload.AddOnStateUpListener(ReloadHold, handTypeForReload);
        GunReload.AddOnStateDownListener(ReloadRelease, handTypeForReload);
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
       
        if (GrabLastFrameForReload)
        {
            RefToHandReload = hand;
            GrabLastFrameForReload = false;
            hand.AttachObject(reloadGameObject, GrabTypes.Scripted);
        }


    }


    private void ReloadRelease(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        
        
            GrabLastFrameForReload = false;
            IsReloading = false;

    }

    private void ReloadHold(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {

        if (RefToHandReload )
        {
            RefToHandReload.DetachObject(reloadGameObject);
            RefToHandReload = null;
        }
        else
        {
            IsReloading = true;
            GrabLastFrameForReload = true;
        }
    }
}
