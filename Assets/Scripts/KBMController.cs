using UnityEngine;
using System.Collections;

public class KBMController : MonoBehaviour {
    Transform myTransform;
    GameObject head;
    public float rotationSpeed = 90;
    public float moveSpeed = 1;


    // Use this for initialization
    void Start () {
        myTransform = GetComponent<Transform>();  
	}

    void Awake()
    {
        head = GetComponentInChildren<NewtonVR.NVRHead>().gameObject;
        if(head.GetComponent<SteamVR_TrackedObject>().isValid)
        {
            Debug.Log("Vive HMD is running, removing keyboard+mouse controller");
            Destroy(this);
            MouseLook mouselook = head.GetComponent<MouseLook>();
            if (mouselook != null) Destroy(mouselook);
        }
    }
	
	// Update is called once per frame
	void Update () {
        bool moveForward = false, moveBack = false, moveLeft = false, moveRight = false;
        if (Input.GetKey(KeyCode.W)) moveForward = true;
        if (Input.GetKey(KeyCode.S)) moveBack = true;
        if (Input.GetKey(KeyCode.A)) moveLeft = true;
        if (Input.GetKey(KeyCode.D)) moveRight = true;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            myTransform.Translate(Vector3.up * -0.05f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            myTransform.Translate(Vector3.up * 0.05f);
        }
        if (moveLeft && !moveRight)
        {
            myTransform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        } else if(moveRight && !moveLeft)
        {
            myTransform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (moveForward && !moveBack)
        {
            myTransform.Translate(0, 0, moveSpeed * Time.deltaTime);
        } else if(moveBack && !moveForward)
        {
            myTransform.Translate(0, 0, -moveSpeed * Time.deltaTime);
        }
        
    }
}
