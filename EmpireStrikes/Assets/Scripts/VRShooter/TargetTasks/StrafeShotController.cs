using System;
using UnityEngine;

public class StrafeShotController : TargetBaseController
{
    public GameObject strafeShotPrefab;
    public Transform spawnPoint;

    private StrafeShotTarget _currentTarget;

    #region External Functions

    public override void RegisterTargetHit()
    {
        base.RegisterTargetHit();

        Destroy(_currentTarget.gameObject);
        SpawnTarget();
    }

    public override void StartGame()
    {
        base.StartGame();
        SpawnTarget();
    }

    public override void EndGame()
    {
        base.EndGame();
        if (_currentTarget != null)
        {
            Destroy(_currentTarget.gameObject);
            _isGameActive = false;
        }
    }

    #endregion

    #region Utility Functions

    private void SpawnTarget()
    {
        GameObject targetInstance = Instantiate(strafeShotPrefab, spawnPoint.position, Quaternion.identity);
        StrafeShotTarget currentTarget = targetInstance.GetComponent<StrafeShotTarget>();

        _currentTarget = currentTarget;
        _currentTarget.SetDefaults(this);
    }

    #endregion
}
