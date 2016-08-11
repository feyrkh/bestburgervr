using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	// Use this for initialization
	void Update () {
        transform.rotation = Quaternion.Euler(0, transform.parent.rotation.eulerAngles.y, 0);
	}
}
