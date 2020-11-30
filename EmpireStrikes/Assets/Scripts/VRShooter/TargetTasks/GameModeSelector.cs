using UnityEngine;

public class GameModeSelector : MonoBehaviour
{
    public GameMode gameMode;
    public GameManager gameManager;

    #region Unity Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Bullet))
        {
            gameManager.StartGame(gameMode);
        }
    }

    #endregion

    #region Enums

    public enum GameMode
    {
        StandingTarget,
        GridShot,
        StrafeShot,
    }

    #endregion
}
