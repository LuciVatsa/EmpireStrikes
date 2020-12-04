using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    private bool bCanDestroy = false;
    public GameObject CurrScore;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<BoxCollider>())
        {
            bCanDestroy = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(bCanDestroy)
        {
            gameObject.SetActive(false);
            CurrScore.GetComponent<ScoreManager>().Score++;
        }
    }
}
