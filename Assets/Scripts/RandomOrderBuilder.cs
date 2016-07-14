using UnityEngine;
using System.Collections;

public class RandomOrderBuilder : OrderBuilder {
    public string[] ingredientsAllowed = new string[] { "meat", "ketchup", "lettuce" };
    public int orderComplexity = 2;

    protected override string[] GenerateNewOrder()
    {
        string[] newOrder = new string[orderComplexity+2];
        newOrder[0] = "bottom_bun";
        newOrder[newOrder.Length - 1] = "top_bun";
        for (int i=1;i<=orderComplexity;i++)
        {
            newOrder[i] = ingredientsAllowed[Random.Range(0, ingredientsAllowed.Length)];
        }
        for(int i=0;i<newOrder.Length;i++)
        {
            Debug.Log("Generated order: " + newOrder[i]);
        }
        return newOrder;
    }
}
