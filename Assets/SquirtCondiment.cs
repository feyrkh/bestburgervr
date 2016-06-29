using UnityEngine;
using System.Collections;

public class SquirtCondiment : MonoBehaviour {

    private float secondsUntilNextSquirt = 0;

    public float secondsBetweenSquirts = 1;

    public GameObject condimentObject;
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(counter == 0 && secondsUntilNextSquirt > 0)
        {
            secondsUntilNextSquirt -= Time.deltaTime;
        } 
	}

    public void DoSquirt(Collider collider)
    {
        Instantiate(condimentObject, transform.position, transform.rotation);
        secondsUntilNextSquirt = secondsBetweenSquirts;  
    }

    private int counter = 0;
    void OnTriggerEnter(Collider other)
    {
        if(counter == 0 && secondsUntilNextSquirt <= 0)
        {
            DoSquirt(other);
        }
        counter++;
    }

    void OnTriggerExit(Collider other)
    {
        counter--;
    }
}
