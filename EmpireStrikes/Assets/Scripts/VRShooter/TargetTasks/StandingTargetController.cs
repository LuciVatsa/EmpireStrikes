using System.Collections.Generic;
using UnityEngine;

public class StandingTargetController : MonoBehaviour
{
    public List<StandingTarget> targets;

    private bool _isGameActive;

    #region Unity Functions

    private void Start()
    {
        foreach (StandingTarget standingTarget in targets)
        {
            standingTarget.SetDefaults(this);
        }
    }

    #endregion

    #region External Functions

    public void RegisterTargetHit()
    {
        List<StandingTarget> validTargets = new List<StandingTarget>();

        foreach (StandingTarget standingTarget in targets)
        {
            if (!standingTarget.IsActive)
            {
                validTargets.Add(standingTarget);
            }
        }

        if (validTargets.Count > 0)
        {
            int randomIndex = Mathf.FloorToInt(Random.value * validTargets.Count);
            validTargets[randomIndex].EnableTarget();
        }
    }

    public void StartGame()
    {
        RegisterTargetHit();
        _isGameActive = true;
    }

    public void EndGame()
    {
        foreach (StandingTarget standingTarget in targets)
        {
            standingTarget.DisableTarget();
        }

        _isGameActive = false;
    }

    public bool IsGameActive => _isGameActive;

    #endregion
}
