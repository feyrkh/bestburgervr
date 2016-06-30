using UnityEngine;
using System.Collections;

public class Magnetic : MonoBehaviour {
    public Collider triggerArea;
    public Vector3 magneticForce = new Vector3(0, -1, 0);
    private Rigidbody rigidBody;
    private bool attracting = false;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(attracting)
        {
            rigidBody.AddRelativeForce(magneticForce,ForceMode.Force);
        }
        attracting = false;

    }
    
    void OnTriggerStay(Collider other)
    {
        if(!rigidBody.IsSleeping())
        {
            attracting = true;
        }
    }
}
