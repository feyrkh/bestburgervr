using UnityEngine;
using System.Collections;

public class TipJar : MonoBehaviour {
    public Transform coinSpawnPoint;
    public GameObject coinPrefab;

    public void SpawnCoins(int tipTotal)
    {
        StartCoroutine("SpawnCoinsCoroutine", tipTotal);
    }

    public IEnumerator SpawnCoinsCoroutine(int tipTotal) { 
        if (coinPrefab != null && coinSpawnPoint != null)
        {
            for (int i = 0; i < tipTotal; i++)
            {
                Debug.Log("Spawned coin", this);
                Vector3 randPos = UnityEngine.Random.insideUnitSphere;
                randPos.Scale(new Vector3(0.05f, 0.05f, 0.05f));
                Vector3 randRot = UnityEngine.Random.insideUnitSphere;
                randRot.Scale(new Vector3(90f, 90f, 90f));
                Instantiate(coinPrefab, coinSpawnPoint.position + randPos, Quaternion.Euler(randRot));
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
