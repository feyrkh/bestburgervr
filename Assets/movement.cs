using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	float x, y, z;
	public float xSpeed, ySpeed, zSpeed;
	void Update () {
		x = Input.GetAxis ("Horizontal") * Time.deltaTime * xSpeed;
		z = Input.GetAxis ("Horizontal") * Time.deltaTime * zSpeed;
		y = Input.GetAxis ("Horizontal") * Time.deltaTime * ySpeed;


	}
}
