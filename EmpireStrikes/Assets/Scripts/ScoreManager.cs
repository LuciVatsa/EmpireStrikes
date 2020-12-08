using UnityEngine.UI;
using UnityEngine;
using Valve.VR;

public class ScoreManager : MonoBehaviour
{
    public int Score = 0;
    public Text ScoreText;

    // a reference to the action
    public SteamVR_Action_Boolean FinishExecute;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = Score.ToString();
        FinishExecute.AddOnStateDownListener(Endgame, handType);
    }
    private void Update()
    {
        ScoreText.text = Score.ToString();
    }

    public void Endgame(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Application.Quit();
    }
}
