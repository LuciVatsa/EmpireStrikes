using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    public int NumberOfTries = 0;
    public Transform[] StartPositions;

    public Transform[] EndPositions;

    public GameObject scoreHandler;

    public GameObject CubetoSpawn;

    private void Start()
    {
        StartCoroutine(Spawn(NumberOfTries));
    }

    IEnumerator Spawn(int NOfT)
    {
        while (NOfT > 0)
        {
            yield return new WaitForSeconds(4f);

            var SpawnedCube = (GameObject)Instantiate(CubetoSpawn, gameObject.transform.position, gameObject.transform.rotation);
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
