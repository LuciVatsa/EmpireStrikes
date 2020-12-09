using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score = 0;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = Score.ToString();
    }
    private void LateUpdate()
    {
        ScoreText.text = Score.ToString();
    }
}
