using UnityEngine;
using System.Collections;
using System;

public class SaveHat : MonoBehaviour {
    public int saveFileId;
    public Color color;
	
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
}
