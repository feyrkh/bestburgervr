using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EarlyBirdWave : WaveRules
{
    public NpcOrder npcPrefab;
    public int waveCount = 3;
    public int npcCount = 2;
    private int currentNpcs = 0;
    public NpcOrder specialOrder;
    public GameObject angryFacePrefab;



    protected override void OnStartLevel()
    {
        Debug.Log("LevelManager has settings: " + LevelManager.Instance.settings);
        waveCount = LevelManager.Instance.settings.customerCount;
        Debug.Log("Starting " + waveCount + " waves");
        StartCoroutine("SpawnWave");
    }

    public override IEnumerator SpawnWave()
    {
        npcCount = LevelManager.Instance.settings.customerCount;
        LevelManager.Instance.orderRules.GenerateOrder(specialOrder);

        waveCount--;
        if (waveCount < 0)
        {
            Debug.Log("Last wave passed, ending level");
            EndLevel();
        }
        else
        {
            currentNpcs = npcCount;
            for (int i = 0; i < npcCount; i++)
            {
                NpcOrder currentNpc = (NpcOrder)Instantiate(npcPrefab, LevelManager.Instance.startPosition.position, LevelManager.Instance.startPosition.rotation);
                currentNpc.GetComponent<NpcEnterStyle>().StartCoroutine("NpcEnter");
                currentNpc.SetOrder(specialOrder.GetOrder());
                yield return new WaitForSeconds(2f);
            }
        }
    }

    public override IEnumerator NpcServed(NpcOrder npc, CompletedBurger burger)
    {
        Debug.Log("Npc was served, sending this one away");
        float accuracy = LevelManager.Instance.burgerScorer.CalcAccuracyScore(specialOrder.GetOrder(), burger.ingredients);
        if (accuracy < 99)
        {
            Debug.Log("Accuracy was bad, making angry face: " + accuracy, burger);
            if (npc.transform.FindChild("angryFace"))
            {
                // If this NPC was already angry, end the level
                EndLevel();
            }
            else
            {
                GameObject angryFace = (GameObject)Instantiate(angryFacePrefab, npc.transform);
                angryFace.transform.localPosition = new Vector3(0, 1f, 0);
                npc.acceptingOrders = true;
            }
            Destroy(burger.gameObject);
        }
        else
        {
            Debug.Log("Customer satisfied");
            //yield return ScoreBurger(npc, burger);
            Destroy(burger);
            npc.GetComponent<NpcEnterStyle>().StopMovement();
            yield return npc.GetComponent<NpcExitStyle>().NpcExit();
            currentNpcs--;
            TipJar tipJar = GameObject.FindObjectOfType<TipJar>();
            if (currentNpcs <= 0)
            {
                float maxCoins = LevelManager.Instance.settings.difficultyLevel;
                maxCoins *= maxCoins;
                maxCoins *= 0.25f;
                maxCoins /= 3;
                maxCoins = Mathf.Max(0.25f, maxCoins);
                if (tipJar != null) yield return tipJar.SpawnCoinsCoroutine(Random.Range(0, maxCoins) + 0.25f);
                Debug.Log("Finished giving tip, spawning next wave");
                StartCoroutine("SpawnWave");
            }
        }
    }
}
