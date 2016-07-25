using UnityEngine;
using System.Collections;

public class AutoFulfilledOrderRules : RandomOrderRules {
    public float secondsToWait = 1;
    public CompletedBurger completedBurgerPrefab;

    public override void GenerateOrder(NpcOrder npc)
    {
        base.GenerateOrder(npc);
        StartCoroutine("AutoFulfillOrder", npc);
    }

    public IEnumerator AutoFulfillOrder(NpcOrder npc)
    {
        yield return new WaitForSeconds(secondsToWait);
        CompletedBurger completedBurger = (CompletedBurger)Instantiate(completedBurgerPrefab, npc.trayPosition.position, Quaternion.identity);
        npc.ReceiveCompletedBurger(completedBurger);
    }

}
