using UnityEngine;
using System.Collections;

public class OrderPlacardDetector : MonoBehaviour {
    Vector3 startPoint;
    public Color detectedColor;
    public Color notDetectedColor;
    int placardsInArea = 0;
    public float timeUntilOrderComplete = 5f;
    float placardResidentSeconds = 0f;

	// Use this for initialization
	void Start () {
        startPoint = GetComponent<Transform>().transform.localPosition;
	}

    void Update()
    {
        if(placardsInArea > 0)
        {
            placardResidentSeconds += Time.deltaTime;
        } else if(placardResidentSeconds > 0)
        {
            placardResidentSeconds = 0;
        }
    }
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Order Placard")
        {
            if(placardsInArea == 0)
            {
                this.GetComponent<MeshRenderer>().material.color = detectedColor;
            }
            placardsInArea++;
            Debug.Log("Placard entered the detection area");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Order Placard")
        {
            placardsInArea--;
            if (placardsInArea == 0)
            {
                this.GetComponent<MeshRenderer>().material.color = notDetectedColor;
            }
            Debug.Log("Placard left the detection area");
        }
    }
   
}
