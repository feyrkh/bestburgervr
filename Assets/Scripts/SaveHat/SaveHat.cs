using UnityEngine;
using System.Collections;
using System;

public class SaveHat : MonoBehaviour {
    public int saveFileId;
    public Color color;
    public SaveHatShelf shelf;
    public int coinCount;
    public bool flairLoaded = false;
	
    public void Awake()
    {
        if (color != null) SetColor(color);
    }
	
    public void OnLevelWasLoaded()
    {
        // Since the flair buttons are stuck to a hat with a joint rather than children of its transform,
        // they get destroyed between level loads. They aren't children because this causes problems with 
        // moving them around with your controllers.
        SaveHatShelf.BuildFlair(this);
    }

    internal void SetColor(Color color)
    {
        this.color = color;

        transform.FindChild("colored").GetComponent<Renderer>().material.color = color;
    }

    public void OnStickyAttached(Transform stickyParent)
    {
        HatFlair flair = stickyParent.GetComponent<HatFlair>();
        if(!flair)
        {
            Debug.LogError("Sticky item was attached, but it's not a HatFlair instance", stickyParent);
            return;
        }
        shelf.AddFlair(this, flair);
    }

    public void OnStickyDetached(Transform stickyParent)
    {
        HatFlair flair = stickyParent.GetComponent<HatFlair>();
        if (!flair)
        {
            Debug.LogError("Sticky item was detached, but it's not a HatFlair instance", stickyParent);
            return;
        }
        shelf.RemoveFlair(this, flair);
    }

    public void OnSceneChanging()
    {
        // Allow the flair to be reloaded when the scene changes
        this.flairLoaded = false;
    }

    public void OnEndInteraction()
    {
        Debug.Log("Dropping a hat, checking to see if there's a nearby HatHanger");
        var overlappingItems = Physics.OverlapSphere(transform.position, 0.05f, -1, QueryTriggerInteraction.Collide);
        for(int i=0;i<overlappingItems.Length;i++)
        {
            Transform item = overlappingItems[i].transform;
            if(item.gameObject.tag == "HatHanger")
            {
                // Do we need to reload the current level after attaching the hat?
                bool reloadLevelAfterAttach = LevelManager.Instance.currentlyLoadedSaveFile != this.saveFileId;
                if (LevelManager.Instance.WearHat(this))
                {
                    Debug.Log("Attaching hat to a hat hanger", item.gameObject);
                    transform.SetParent(item);
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    if(reloadLevelAfterAttach)
                    {
                        LevelManager.Instance.ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, false, 0.5f);
                    }
                    return;
                } else
                {
                    Debug.Log("Already wearing a hat");
                }
            }
            Debug.Log("Dropping a hat");
            transform.SetParent(null);
            transform.GetComponent<Rigidbody>().isKinematic = false;
            LevelManager.Instance.RemoveHat(this);
        }
    }
}
