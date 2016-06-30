using UnityEngine;
using System.Collections;

public class ColorMesh : MonoBehaviour {
    public Color color;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
