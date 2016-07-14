using UnityEngine;
using System.Collections;

public class OrderBuilder : MonoBehaviour
{
    public NpcOrder npcOrder;

    public void BuildNewOrder()
    {
        var newOrder = GenerateNewOrder();
        if (npcOrder == null)
        {
            npcOrder = GetComponent<NpcOrder>();
            if (npcOrder.GetOrder() == null)
            {
                npcOrder.SetOrder(newOrder);
            }
        }
    }

    protected virtual string[] GenerateNewOrder()
    {
        return new string[] { "bottom_bun", "meat", "tomato", "top_bun" };
    }
}