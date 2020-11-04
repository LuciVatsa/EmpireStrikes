using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StandingTargetController targetController;

    #region External Functions

    public void StartGame() => targetController.StartGame();

    public void EndGame() => targetController.EndGame();

    #endregion

    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
