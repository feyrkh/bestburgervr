using UnityEngine;
using System.Collections;
using System;

public class SaveHat : MonoBehaviour {
    public int saveFileId;
    public Color color;
    public SaveHatShelf shelf;
	
    public void Awake()
    {
        if (color != null) SetColor(color);
    }
	
	// Update is called once per frame
	void Update () {
	
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
}
