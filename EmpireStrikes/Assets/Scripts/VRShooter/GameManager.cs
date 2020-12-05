using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public StandingTargetController standingTargetController;
    public GridShotController gridShotController;
    public StrafeShotController strafeShotController;

    [Header("Steam VR")]
    public SteamVR_Action_Boolean endGame;

    [Header("Display Targets")]
    public List<GameObject> modeSelectorObjects;

    private TargetBaseController _targetBaseController;

    #region Unity Functions

    private void Start()
    {
        endGame.AddOnStateDownListener(HandleEndGamePressed, SteamVR_Input_Sources.Any);
    }

    #endregion

    #region External Functions

    public void StartGame(GameModeSelector.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameModeSelector.GameMode.StandingTarget:
                _targetBaseController = standingTargetController;
                break;

            case GameModeSelector.GameMode.GridShot:
                _targetBaseController = gridShotController;
                break;

            case GameModeSelector.GameMode.StrafeShot:
                _targetBaseController = strafeShotController;
                break;
        }

        foreach (GameObject modeSelectorObject in modeSelectorObjects)
        {
            modeSelectorObject.SetActive(false);
        }

        _targetBaseController.StartGame();
    }

    public void EndGame()
    {
        _targetBaseController.EndGame();
        foreach (GameObject modeSelectorObject in modeSelectorObjects)
        {
            modeSelectorObject.SetActive(true);
        }

        _targetBaseController = null;
    }

    #endregion

    #region Utility Functions

    private void HandleEndGamePressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) => EndGame();

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
