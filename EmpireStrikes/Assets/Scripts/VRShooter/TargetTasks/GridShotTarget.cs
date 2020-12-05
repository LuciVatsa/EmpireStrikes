using UnityEngine;

public class GridShotTarget : MonoBehaviour
{
    private TargetBaseController _targetController;
    private bool _isActive;

    #region Unity Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Bullet))
        {
            _targetController.RegisterTargetHit();
            DisableTarget();
        }
    }

    #endregion

    #region External Functions

    public void SetDefaults(TargetBaseController targetController)
    {
        _targetController = targetController;
    }

    public void EnableTarget()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void DisableTarget()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }

    public bool IsActive => _isActive;

    #endregion
}
