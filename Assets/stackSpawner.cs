using UnityEngine;
using System.Collections;

public class stackSpawner : MonoBehaviour {
    public GameObject spawnItem;
    public int spawnCount;
	// Use this for initialization
	void Start () {
        Vector3 position = transform.position + new Vector3(0, transform.lossyScale.y, 0);
        Vector3 itemSize = Vector3.zero;
        if (spawnItem.GetComponent<Renderer>() != null)
        {
            itemSize = spawnItem.GetComponent<Renderer>().bounds.size;
        } else
        {
            itemSize = Vector3.zero;
            Renderer[] childRenderers = spawnItem.GetComponentsInChildren<Renderer>();
            foreach(var r in childRenderers)
            {
                itemSize += r.bounds.size;
            }
        }

        for (int i=0;i<spawnCount;i++)
        {
            GameObject newItem = (GameObject)Instantiate(spawnItem, position, Quaternion.identity);
            position += new Vector3(0, itemSize.y, 0);
        }
	}
}
