using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    private bool bCanDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<BoxCollider>())
        {
            bCanDestroy = true;
            Debug.Log("Has Box Collider");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(bCanDestroy)
        {
            Destroy(gameObject);
            GetComponent<ScoreManager>().Score++;
        }
    }
}
