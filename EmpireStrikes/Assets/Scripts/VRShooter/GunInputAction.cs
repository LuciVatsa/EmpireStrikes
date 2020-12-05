using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunInputAction : MonoBehaviour
{
    [Header("Components")]
    public FireBullets fireBullets;
    public Transform magazineAttachPoint;

    [Header("Steam VR")]
    public SteamVR_Action_Boolean gunGrab;
    public SteamVR_Action_Boolean gunShoot;
    public SteamVR_Input_Sources handType;
    public SteamVR_Input_Sources handTypeForReload;

    private bool _grabLastFrame = false;
    private bool _canFire;

    private Hand _refToHand = null;

    #region Unity Functions

    void Start()
    {
        gunGrab.AddOnStateUpListener(TriggerUp, handType);
        gunGrab.AddOnStateDownListener(TriggerDown, handType);
        gunShoot.AddOnStateUpListener(ShootTriggerUp, handType);
        gunShoot.AddOnStateDownListener(ShootTriggerDown, handType);
    }

    private void LateUpdate()
    {
        if (_refToHand && _canFire)
        {
            bool fireSuccess = fireBullets.Fire();
            if (!fireSuccess)
            {
                _canFire = false;
            }
        }
    }

    #endregion

    #region External Functions

    public void AddMagazine(Reload reload)
    {
        reload.transform.SetParent(magazineAttachPoint);
        reload.transform.localPosition = Vector3.zero;
        reload.transform.localRotation = Quaternion.identity;
        reload.transform.localScale = Vector3.one * 3.5f;

        Destroy(reload.GetComponent<Interactable>());
        Destroy(reload.GetComponent<BoxCollider>());
        Destroy(reload.GetComponent<Rigidbody>());

        fireBullets.AddMag(reload);
    }

    #endregion

    #region Utility Functions

    private void ShootTriggerDown(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        _canFire = true;
    }

    private void ShootTriggerUp(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        _canFire = false;
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (_grabLastFrame)
        {
            _refToHand = hand;
            _grabLastFrame = false;

            hand.AttachObject(this.gameObject, GrabTypes.Scripted);
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void TriggerUp(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        _grabLastFrame = false;
    }

    private void TriggerDown(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {

        if (_refToHand)
        {
            _refToHand.DetachObject(this.gameObject);
            _refToHand = null;

            GameManager.Instance.EndGame();
        }
        else
        {

            _grabLastFrame = true;
        }
    }

    #endregion

}