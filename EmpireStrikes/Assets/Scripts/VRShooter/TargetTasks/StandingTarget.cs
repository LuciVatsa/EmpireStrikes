using UnityEngine;

public class StandingTarget : MonoBehaviour
{
    private const int BottomAngle = 90;
    private const int TopAngle = 0;

    public float lerpSpeed = 1;

    private float _lerpAmount;
    private bool _lerpActive;
    private float _startAngle;
    private float _targetAngle;

    private TargetBaseController _targetController;
    private bool _isActive;

    #region Unity Functions

    private void Update()
    {
        if (!_lerpActive)
        {
            return;
        }

        _lerpAmount += lerpSpeed * Time.deltaTime;
        float mappedAngle = Mathf.LerpAngle(_startAngle, _targetAngle, _lerpAmount);
        transform.rotation = Quaternion.Euler(Vector3.right * mappedAngle);

        if (_lerpAmount >= 1)
        {
            _lerpActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Bullet))
        {
            AddBulletHit();
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
        _isActive = true;
        _lerpActive = true;

        _startAngle = transform.rotation.eulerAngles.x;
        _targetAngle = TopAngle;
        _lerpAmount = 0;
    }

    public void DisableTarget()
    {
        _isActive = false;
        _lerpActive = true;

        _startAngle = transform.rotation.eulerAngles.x;
        _targetAngle = BottomAngle;
        _lerpAmount = 0;
    }

    public bool IsActive => _isActive;

    #endregion

    #region Utility Functions

    private void AddBulletHit()
    {
        if (!_isActive)
        {
            return;
        }

        DisableTarget();
        _targetController.RegisterTargetHit();
    }

    #endregion
}
