using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunInputAction : MonoBehaviour
{

    public SteamVR_Action_Boolean GunGrab;

    public SteamVR_Action_Boolean GunShoot;

    [SerializeField] private FireBullets fireBullets;


    public SteamVR_Input_Sources handType;


    private bool GrabLastFrame = false;
    private bool CanFire;

    private Hand RefToHand = null;
    // Start is called before the first frame update
    void Start()
    {
        GunGrab.AddOnStateUpListener(TriggerUp, handType);
        GunGrab.AddOnStateDownListener(TriggerDown, handType);
        //GunShoot.AddOnUpdateListener(UpdateShoot, handType);
        GunShoot.AddOnStateUpListener(ShootTriggerUp, handType);
        GunShoot.AddOnStateDownListener(ShootTriggerDown, handType);
    

    }

    private void ShootTriggerDown(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        CanFire = true;
    }

    private void ShootTriggerUp(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        CanFire = false;
    }


    private void LateUpdate()
    {
        if (RefToHand&& CanFire)
        {
            fireBullets.Fire();
        }
    }

    private void UpdateShoot(SteamVR_Action_Single fromaction, SteamVR_Input_Sources fromsource, float newaxis, float newdelta)
    {
        if (RefToHand)
        {
            Debug.Log("fiiring"+ newaxis);
        }
    }



    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (GrabLastFrame)
        {
            RefToHand = hand;
            GrabLastFrame = false;
            hand.AttachObject(this.gameObject, GrabTypes.Scripted);
        }


    }

    void TriggerUp(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {

        GrabLastFrame = false;
    }

    void TriggerDown(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {

        if (RefToHand)
        {
            RefToHand.DetachObject(this.gameObject);
            RefToHand = null;
        }
        else
        {
            
            GrabLastFrame = true;
        }

    }

}