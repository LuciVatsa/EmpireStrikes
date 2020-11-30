using UnityEngine;
using Random = UnityEngine.Random;

public class StrafeShotTarget : MonoBehaviour
{
    private int MAX_HIT_COUNT = 3;

    public float minXRange;
    public float maxXRange;
    public float minSwitchTime;
    public float maxSwitchTime;
    public float movementSpeed;

    private TargetBaseController _targetController;
    private float _currentTime;
    private int _directionMultiplier;

    private Material _targetMaterial;
    private int _currentHitCount;

    #region Unity Functions

    private void Start()
    {
        _currentTime = 0;
        if (Random.value <= 0.5f)
        {
            _directionMultiplier = -1;
        }
        else
        {
            _directionMultiplier = 1;
        }

        _targetMaterial = GetComponent<MeshRenderer>().material;
        _targetMaterial.color = Color.green;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            _currentTime = Random.Range(minSwitchTime, maxSwitchTime);
            _directionMultiplier *= -1;
        }

        Vector3 position = transform.position;
        position.x += _directionMultiplier * movementSpeed * Time.deltaTime;
        transform.position = position;

        if (position.x < minXRange || position.x > maxXRange)
        {
            _directionMultiplier *= -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Bullet))
        {
            _currentHitCount += 1;
            if (_currentHitCount >= MAX_HIT_COUNT)
            {
                _targetController.RegisterTargetHit();
            }

            _targetMaterial.color = _currentHitCount == 1 ? Color.yellow : Color.red;
        }
    }

    #endregion

    #region External Functions

    public void SetDefaults(TargetBaseController targetController)
    {
        _targetController = targetController;
    }

    #endregion
}
