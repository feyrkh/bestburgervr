using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour {
    public Transform target;

    public void Awake()
    {
        target = FindObjectOfType<SteamVR_Camera>().transform;
    }

	void Update () {
        this.transform.LookAt(target);
	}
}
