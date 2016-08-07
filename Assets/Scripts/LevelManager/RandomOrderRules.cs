using UnityEngine;
using System.Collections;
using System;

public class RandomOrderRules : OrderRules {
    public string[] ingredientsAllowed = new string[] { "meat", "ketchup", "lettuce", "tomato" };
    private int orderComplexity = 2;

    public override void GenerateOrder(NpcOrder npc)
    {
        orderComplexity = LevelManager.Instance.orderRules.CalculateCurrentOrderComplexity();
        string[] newOrder = new string[orderComplexity + 2];
        newOrder[0] = "bottom_bun";
        newOrder[newOrder.Length - 1] = "top_bun";
        for (int i = 1; i <= orderComplexity; i++)
        {
            newOrder[i] = ingredientsAllowed[UnityEngine.Random.Range(0, ingredientsAllowed.Length)];
        }
        for (int i = 0; i < newOrder.Length; i++)
        {
            Debug.Log("Generated order: " + newOrder[i], npc);
        }
        npc.SetOrder(newOrder);
    }
}
