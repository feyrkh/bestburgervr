using UnityEngine;
using System.Collections;

// Create an NPC prefab, move it to the correct position, give it an order
// When the NPC is destroyed, do it again
public class NpcSpawner : MonoBehaviour {
    public OrderBuilder npcPrefab;
    public OrderBuilder currentNpc;


    void Awake () {
        SpawnNpc(npcPrefab);	
	}

    public void SpawnNpc()
    {
        SpawnNpc(npcPrefab);
    }

    public void SpawnNpc(OrderBuilder npcPrefab)
    {
        currentNpc = (OrderBuilder)Instantiate(npcPrefab, transform.position + new Vector3(-3, 0, 0), Quaternion.identity);
        // Add an NpcRespawner to the new NPC so it gets recreated after being destroyed
        currentNpc.gameObject.AddComponent<InfiniteNpcRespawner>();
        currentNpc.GetComponent<InfiniteNpcRespawner>().spawner = this;
        StartCoroutine("MoveNpcInAndOrder");
    }

    public IEnumerator MoveNpcInAndOrder()
    {
        yield return MoveUtil.MoveOverSeconds(currentNpc.gameObject, transform.position, 1);
        currentNpc.BuildNewOrder();
    }
}


public class InfiniteNpcRespawner : MonoBehaviour
{
    internal NpcSpawner spawner;

    public void OnTimeToLeave()
    {
        spawner.SpawnNpc();
    }
}