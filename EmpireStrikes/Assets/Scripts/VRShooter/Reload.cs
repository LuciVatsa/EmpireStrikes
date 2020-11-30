using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Reload : MonoBehaviour
{
    public SteamVR_Action_Boolean GunReload;
    public SteamVR_Input_Sources handTypeForReload;

    public bool GrabLastFrameForReload = false;

    private Hand RefToHandReload = null;
    public int BulletCount;

    #region Unity Functions

    private void Start()
    {
        BulletCount = 30;
        GunReload.AddOnStateUpListener(ReloadHold, handTypeForReload);
        GunReload.AddOnStateDownListener(ReloadRelease, handTypeForReload);
    }

    private void OnCollisionEnter(Collision other)
    {
        GunInputAction gunInput = other.gameObject.GetComponent<GunInputAction>();
        if (gunInput != null)
        {
            RefToHandReload.DetachObject(gameObject);
            RefToHandReload = null;
            GrabLastFrameForReload = false;

            gunInput.AddMagazine(this);
        }
    }

    #endregion

    #region External Functions

    public bool HasBullets() => BulletCount > 0;

    public void UseBullet() => BulletCount -= 1;

    #endregion

    #region Utility Functions

    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (GrabLastFrameForReload)
        {
            RefToHandReload = hand;
            GrabLastFrameForReload = false;
            hand.AttachObject(this.gameObject, GrabTypes.Scripted);
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void ReloadRelease(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource) => GrabLastFrameForReload = false;

    private void ReloadHold(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource)
    {
        if (RefToHandReload)
        {
            RefToHandReload.DetachObject(gameObject);
            RefToHandReload = null;
        }
        else
        {
            GrabLastFrameForReload = true;
        }
    }

    #endregion
}
