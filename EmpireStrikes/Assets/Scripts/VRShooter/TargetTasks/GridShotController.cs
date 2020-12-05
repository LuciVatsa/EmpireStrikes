using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class GridShotController : TargetBaseController
{
    private const int MAX_ACTIVE_TARGETS = 3;

    public List<GridShotTarget> gridShotTargets;

    #region Unity Functions

    private void Start()
    {
        foreach (GridShotTarget gridShotTarget in gridShotTargets)
        {
            gridShotTarget.SetDefaults(this);
            gridShotTarget.DisableTarget();
        }
    }

    #endregion

    #region External Functions

    public override void RegisterTargetHit()
    {
        base.RegisterTargetHit();
        EnableTargets(1);
    }

    public override void StartGame()
    {
        base.StartGame();
        EnableTargets(MAX_ACTIVE_TARGETS);
    }

    public override void EndGame()
    {
        base.EndGame();

        foreach (GridShotTarget gridShotTarget in gridShotTargets)
        {
            gridShotTarget.DisableTarget();
        }
    }

    #endregion

    #region Utility Functions

    private void EnableTargets(int targetCount)
    {
        List<GridShotTarget> validTargets = new List<GridShotTarget>();
        foreach (GridShotTarget gridShotTarget in gridShotTargets)
        {
            if (!gridShotTarget.IsActive)
            {
                validTargets.Add(gridShotTarget);
            }
        }
        validTargets = validTargets.OrderBy(_ => Random.value <= 0.5f).ToList();

        for (int i = 0; i < targetCount; i++)
        {
            validTargets[i].EnableTarget();
        }
    }

    #endregion
}
