using UnityEngine;
using System.Collections;
using Valve.VR;

public class CubeSpawner : MonoBehaviour
{
    public int NumberOfTries = 20;

    public Transform[] StartPositions;

    public Transform[] EndPositions;

    public GameObject scoreHandler;

    public GameObject CubetoSpawn;

    // a reference to the action
    public SteamVR_Action_Boolean RestartGame;
    
    // a reference to the hand
    public SteamVR_Input_Sources handType;

    private void Start()
    {
        RestartGame.AddOnStateDownListener(Restart, handType);
    }

    public void Restart(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StartCoroutine(Spawn(NumberOfTries));
    }
    IEnumerator Spawn(int NOfT)
    {
        scoreHandler.GetComponent<ScoreManager>().Score = 0;

        yield return new WaitForSeconds(5f);

        while (NOfT > 0)
        {
            yield return new WaitForSeconds(1f);
            var SpawnedCube = Instantiate(CubetoSpawn, gameObject.transform.position, gameObject.transform.rotation);
            SpawnedCube.AddComponent<BoxMovement>();
            SpawnedCube.AddComponent<CubeDestroyer>();
            SpawnedCube.AddComponent<ScoreManager>();
            SpawnedCube.GetComponent<BoxMovement>().StartPos = StartPositions[(int)Random.Range(0.0f, StartPositions.Length - 1)];
            SpawnedCube.GetComponent<BoxMovement>().EndPos = EndPositions[(int)Random.Range(0.0f, StartPositions.Length - 1)];
            SpawnedCube.GetComponent<BoxMovement>().speed = Random.Range(2.0f, 4.0f);
            SpawnedCube.GetComponent<CubeDestroyer>().CurrScore = scoreHandler;
            NOfT--;
        }
        yield return null;
    }
}
