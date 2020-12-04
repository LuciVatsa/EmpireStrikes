using Valve.VR;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public Transform[] StartPositions;

    public Transform[] EndPositions;
    // a reference to the action
    public SteamVR_Action_Boolean SpawnCube;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    public GameObject CubetoSpawn;

    private void Start()
    {
        SpawnCube.AddOnStateDownListener(CubeMagic, handType);
    }

    public void CubeMagic(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        var SpawnedCube = (GameObject)Instantiate(CubetoSpawn, gameObject.transform.position, gameObject.transform.rotation);
        SpawnedCube.AddComponent<BoxMovement>();
        SpawnedCube.AddComponent<CubeDestroyer>();
        SpawnedCube.AddComponent<ScoreManager>();
        SpawnedCube.GetComponent<BoxMovement>().StartPos = StartPositions[(int)Random.Range(0.0f, StartPositions.Length - 1)];
        SpawnedCube.GetComponent<BoxMovement>().EndPos = EndPositions[(int)Random.Range(0.0f, StartPositions.Length - 1)];
        SpawnedCube.GetComponent<BoxMovement>().speed = Random.Range(2.0f, 4.0f);
    }
}
