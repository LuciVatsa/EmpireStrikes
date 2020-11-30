using System.Collections.Generic;
using UnityEngine;

public class StandingTargetController : TargetBaseController
{
    public List<StandingTarget> targets;

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

    public override void RegisterTargetHit()
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

    public override void StartGame()
    {
        RegisterTargetHit();
        _isGameActive = true;
    }

    public override void EndGame()
    {
        foreach (StandingTarget standingTarget in targets)
        {
            standingTarget.DisableTarget();
        }

        _isGameActive = false;
    }

    #endregion
}
