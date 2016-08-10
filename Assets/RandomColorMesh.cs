using UnityEngine;
using System.Collections;

public class RandomColorMesh : MonoBehaviour {
    public Color color;

    // Use this for initialization
    void Awake()
    {
        color = Random.ColorHSV();
        gameObject.GetComponent<Renderer>().material.color = color;
        Destroy(this);
    }
}
