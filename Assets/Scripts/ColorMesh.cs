using UnityEngine;
using System.Collections;

public class ColorMesh : MonoBehaviour {
    public Color color;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.color = color;
        Destroy(this);
    }
}
