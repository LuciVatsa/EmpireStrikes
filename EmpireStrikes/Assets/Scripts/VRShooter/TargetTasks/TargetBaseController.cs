using UnityEngine;

public class TargetBaseController : MonoBehaviour
{
    protected bool _isGameActive;

    #region External Functions

    public virtual void RegisterTargetHit()
    {

    }

    public virtual void StartGame()
    {

    }

    public virtual void EndGame()
    {

    }

    public bool IsGameActive => _isGameActive;

    #endregion
}
