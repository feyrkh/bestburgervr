using UnityEngine;
using System.Collections;

public class stackSpawner : MonoBehaviour {
    public GameObject spawnItem;
    public int spawnCount;
    public float extraOffset;
    public Vector3 rotation = new Vector3(90, 0, 0);
	// Use this for initialization
	void Update () {
        if(transform.parent.GetComponent<Rigidbody>().IsSleeping()) { 
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
                position += new Vector3(0, itemSize.y+extraOffset, 0);
                newItem.transform.localRotation = Quaternion.Euler(rotation);
                //newItem.GetComponent<Rigidbody>().Sleep();
            }
            Destroy(this);
        }
    }
}
