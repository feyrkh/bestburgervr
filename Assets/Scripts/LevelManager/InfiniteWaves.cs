using UnityEngine;
using System.Collections;
using System;

public class InfiniteWaves : WaveRules
{
    public NpcOrder npcPrefab;
    NpcOrder currentNpc;

    public override IEnumerator SpawnWave()
    {
        currentNpc = (NpcOrder)Instantiate(npcPrefab, LevelManager.Instance.startPosition.position, LevelManager.Instance.startPosition.rotation);
        yield return LevelManager.Instance.npcEnterStyle.NpcEnter(currentNpc.transform);
        LevelManager.Instance.orderRules.GenerateOrder(currentNpc);
    }

    public override IEnumerator NpcServed(NpcOrder npc, CompletedBurger burger)
    {
        Debug.Log("Npc was served, sending this one away then spawning a new wave");
        yield return ScoreBurger(npc, burger);
        yield return LevelManager.Instance.npcExitStyle.NpcExit(npc.transform);
        Debug.Log("Npc exited, spawning a new wave");
        StartCoroutine("SpawnWave");
    }

    private IEnumerator ScoreBurger(NpcOrder npc, CompletedBurger burger)
    {
        GameObject tray = burger.container;
        Transform trayPosition = npc.trayPosition;
        yield return MoveUtil.MoveOverSeconds(tray, trayPosition.localPosition, 1f);
        HappinessScoreDisplay scorer = Instantiate<HappinessScoreDisplay>(npc.scorePrefab);
        scorer.transform.SetParent(npc.transform);
        scorer.transform.localPosition = npc.scorePosition.localPosition;
        scorer.transform.localEulerAngles = npc.scorePosition.localEulerAngles;
        if (scorer != null)
        {
            yield return scorer.ScoreOrder(npc.GetDesiredIngredients(), burger, npc.GetTimeSinceOrderStarted());
        }
        Destroy(scorer);
    }

    protected override void OnStartLevel()
    {
        Debug.Log("Starting an infinite # of waves");
        StartCoroutine("SpawnWave");
    }

    protected override void OnEndLevel()
    {
    }
}
