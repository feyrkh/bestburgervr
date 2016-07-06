using UnityEngine;
using System.Collections;

public class RandomOrderBuilder : MonoBehaviour {
    public NpcOrder npcOrder;
    public string[] ingredientsAllowed = new string[] { "meat", "ketchup", "lettuce" };
    public int orderComplexity = 2;
     
	// Use this for initialization
	void Start () {
	    if(npcOrder == null)
        {
            npcOrder = GetComponent<NpcOrder>();
        }
        StartCoroutine("RandomOrder");
	}

    public IEnumerator RandomOrder()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.25f);
            BuildRandomOrder();
        }
    }

    public void BuildRandomOrder()
    {
        if(npcOrder.GetOrder() == null)
        {
            string[] newOrder = new string[orderComplexity];
            for(int i=0;i<orderComplexity;i++)
            {
                newOrder[i] = ingredientsAllowed[Random.Range(0, ingredientsAllowed.Length)];
            }
            npcOrder.SetOrder(newOrder);
        }
    }
}
