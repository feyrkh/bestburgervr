using UnityEngine;
using System.Collections;

public class LunchRushWave : WaveRules
{

    public NpcOrder npcPrefab;
    
    private int currentNpcs = 0;
    public GameObject angryFacePrefab;
    public Transform[] lunchRushOrderPositions = new Transform[3];
    public NpcOrder[] customers;
    public int timeToRun = 180;
    public ClockTimer clock;

    protected override void OnStartLevel()
    {
        Debug.Log("LevelManager has settings: " + LevelManager.Instance.settings);
        customers = new NpcOrder[lunchRushOrderPositions.Length];
        Debug.Log("Starting infinite waves for "+timeToRun+" seconds");
        StartCoroutine("SpawnWave");
    }

    public override IEnumerator SpawnWave()
    {
        while(clock.isRunning) {
            yield return new WaitForSeconds(Random.Range(1, 5));

        }
    }

    public override IEnumerator NpcServed(NpcOrder npc, CompletedBurger burger)
    {
        Debug.Log("Npc was served, sending this one away");
        float accuracy = LevelManager.Instance.burgerScorer.CalcAccuracyScore(npc.GetOrder(), burger.ingredients);
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
                angryFace.name = "angryFace";
                npc.acceptingOrders = true;
            }
            Destroy(burger.gameObject);
        }
        else
        {
            Debug.Log("Customer satisfied");
            Transform clock = npc.transform.FindChild("clock");
            if (clock != null) Destroy(clock.gameObject);
            Destroy(burger);
            npc.GetComponent<NpcEnterStyle>().StopMovement();
            yield return npc.GetComponent<NpcExitStyle>().NpcExit();
            currentNpcs--;
            TipJar tipJar = GameObject.FindObjectOfType<TipJar>();
            if (currentNpcs <= 0)
            {
                float maxCoins = LevelManager.Instance.settings.difficultyLevel;
               // maxCoins += npcCount * 0.1f;
                maxCoins *= maxCoins;
                maxCoins *= 0.25f;
                maxCoins /= 3;
                maxCoins = Mathf.Max(0.25f, maxCoins);
                if (tipJar != null) tipJar.SpawnCoins(Random.Range(0, maxCoins) + 0.25f);
                Debug.Log("Finished giving tip, spawning next wave");
                StartCoroutine("SpawnWave");
            }
        }
    }
}
