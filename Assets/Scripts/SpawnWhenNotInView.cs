using UnityEngine;
using System.Collections;
using System;

public class SpawnWhenNotInView : MonoBehaviour {

    public Renderer myRenderer;
    public Transform spawnPoint;
    public Collider detectArea;
    public GameObject spawnPrefab;
    public string spawnedTag = "spawned";
    public float pauseBetweenSpawns = 1f;
    public int desiredSpawnCount = 1;

    void Awake()
    {
        if (myRenderer != null) StartCoroutine("SpawnToothpicks");
    }

    public IEnumerator SpawnToothpicks()
    {
        while (true)
        {
            yield return new WaitForSeconds(pauseBetweenSpawns);
            if (!myRenderer.isVisible) TrySpawnItem();
        }
    }

    private void TrySpawnItem()
    {
        
        Bounds bounds = detectArea.bounds;
        Collider[] hits = Physics.OverlapBox(bounds.center, bounds.extents, detectArea.transform.rotation);
        int spawnsFound = 0;
        for(int i=0;i<hits.Length;i++) {
            Transform item = hits[i].transform;
            while (item.parent != null) item = item.parent;
            if (spawnedTag == item.tag) spawnsFound++;
        }
//        Debug.Log("Found " + hits.Length + " items, " + spawnsFound + " spawns");
        if(spawnsFound < desiredSpawnCount)
        {
            GameObject item = (GameObject)Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
            item.tag = spawnedTag;
        }
    }
}
