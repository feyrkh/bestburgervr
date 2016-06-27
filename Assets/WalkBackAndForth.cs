using UnityEngine;
using System.Collections;

public class WalkBackAndForth : MonoBehaviour {
    public Transform startPoint;
    public Transform endPoint;
    public float secondsToTraverse = 10;
    private Vector3 direction;
    private float distancePerSecond;
    private float secondsPassed = 0;
    
	// Use this for initialization
	void Start () {
        NewPosition();
    }
	
    void NewPosition()
    {
        secondsPassed = 0;
        this.transform.position = startPoint.transform.position;
        direction = (endPoint.transform.position - startPoint.transform.position);
    }

    // Update is called once per frame
    void Update () {
        this.transform.Translate(direction * (Time.deltaTime / secondsToTraverse));
        secondsPassed += Time.deltaTime;
        if (secondsPassed >= secondsToTraverse) {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;            
            Transform swap = endPoint;
            endPoint = startPoint;
            startPoint = swap;
            NewPosition();
        }
    }
}
