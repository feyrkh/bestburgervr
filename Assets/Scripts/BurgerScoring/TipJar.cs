using UnityEngine;
using System.Collections;

public class TipJar : MonoBehaviour {
    public Transform coinSpawnPoint;
    public GameObject coinPrefab;
    public Coin quarterDollarCoinPrefab;
    public Coin oneDollarCoinPrefab;
    public Coin fiveDollarCoinPrefab;
    public Coin twentyDollarCoinPrefab;

    public void SpawnCoins(float tipTotal)
    {
        StartCoroutine("SpawnCoinsCoroutine", tipTotal);
    }

    public IEnumerator SpawnCoinsCoroutine(float tipTotal) {
        Coin coinPrefab;
        Debug.Log("Spawning coins: " + tipTotal);
        while (tipTotal > 0.12f)
        {
            if (tipTotal >= 20) coinPrefab = twentyDollarCoinPrefab;
            else if (tipTotal >= 5) coinPrefab = fiveDollarCoinPrefab;
            else if (tipTotal >= 1) coinPrefab = oneDollarCoinPrefab;
            else coinPrefab = quarterDollarCoinPrefab;
            tipTotal -= coinPrefab.coinValue;
            yield return SpawnCoin(coinPrefab);
        }
    }

    public IEnumerator SpawnCoin(Coin coinPrefab) {
        if (coinPrefab != null && coinSpawnPoint != null)
        {
            Debug.Log("Spawned coin", coinPrefab);
            Vector3 randPos = UnityEngine.Random.insideUnitSphere;
            randPos.Scale(new Vector3(0.05f, 0.05f, 0.05f));
            Vector3 randRot = UnityEngine.Random.insideUnitSphere;
            randRot.Scale(new Vector3(90f, 90f, 90f));
            Instantiate(coinPrefab, coinSpawnPoint.position + randPos, Quaternion.Euler(randRot));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
