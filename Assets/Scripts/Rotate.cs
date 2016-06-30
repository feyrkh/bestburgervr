using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float xDegreesPerSecond = 0;
    public float yDegreesPerSecond = 0;
    public float zDegreesPerSecond = 0;
    private Transform transform;

	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(xDegreesPerSecond * Time.deltaTime, yDegreesPerSecond * Time.deltaTime, zDegreesPerSecond * Time.deltaTime);
	}
}
