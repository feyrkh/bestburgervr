using UnityEngine;
using System.Collections;

public class CondimentGlob : MonoBehaviour {
    public float thickness = 0.01f;
    public float condimentAmount = 0.03f;
    public float maxCondimentAmount = 0.5f;
    public string condimentName = "ketchup";
    bool attached = false;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision col)
    {
        if (attached) return;
        GameObject other = col.gameObject;
        CondimentGlob otherCondiment = other.GetComponent<CondimentGlob>();
        if(otherCondiment != null && otherCondiment.condimentName.Equals(condimentName)) 
        {
            // merge with condiment glob
            //otherCondiment.condimentAmount = Mathf.Min(otherCondiment.condimentAmount + condimentAmount, otherCondiment.maxCondimentAmount);
            //otherCondiment.gameObject.transform.localScale = new Vector3(otherCondiment.condimentAmount, otherCondiment.transform.localScale.y, otherCondiment.condimentAmount);
            Destroy(gameObject);
        } else
        {
             if (other.GetComponent<CondimentSticky>() == null) return;
            /*
            transform.localScale = new Vector3(condimentAmount, thickness, condimentAmount);
            FixedJoint attachPoint = gameObject.AddComponent<FixedJoint>();
            attachPoint.autoConfigureConnectedAnchor = false;
            attachPoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
            
            Debug.Log("Point of contact: " + col.contacts[0].point);
            attachPoint.connectedAnchor = col.contacts[0].point;
            attachPoint.anchor = new Vector3(0, -thickness / 2, 0);
            */
            transform.SetParent(other.transform);
            //FixedJoint attachPoint = gameObject.AddComponent<FixedJoint>();
            //attachPoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
            attached = true;
            //Destroy(GetComponent<Collider>());
            Destroy(GetComponent<Rigidbody>());
        }
    }
}
