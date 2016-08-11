using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	// Use this for initialization
	void FixedUpdate () {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}
}
