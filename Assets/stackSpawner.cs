using UnityEngine;
using System.Collections;

public class stackSpawner : MonoBehaviour {
    public GameObject spawnItem;
    public int spawnCount;
	// Use this for initialization
	void Start () {
        Vector3 position = transform.position + new Vector3(0, transform.lossyScale.y, 0);
        for(int i=0;i<spawnCount;i++)
        {
            GameObject newItem = (GameObject)Instantiate(spawnItem, position, Quaternion.identity);
            position += new Vector3(0, newItem.transform.localScale.y, 0);
        }
	}
}
