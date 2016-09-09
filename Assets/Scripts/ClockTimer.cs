using UnityEngine;
using System.Collections;

public class ClockTimer : MonoBehaviour {

	public float secondsToRun = 30;
	private float secondsElapsed = 0;
	public Transform hand;
    public bool restart = true;
    public bool isRunning = true;

	// Use this for initialization
	void Start () {
	    isRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (secondsElapsed < secondsToRun) {
			secondsElapsed += Time.deltaTime;
			if (secondsElapsed > secondsToRun)
				secondsElapsed = secondsToRun;
			if (secondsElapsed >= secondsToRun) {
				SendMessageUpwards ("OnClockTimerElapsed", this);
                isRunning = false;
                if (restart) ResetClock();
			}
			hand.localRotation = Quaternion.Euler(new Vector3 (0, 0, 360 * (secondsElapsed / secondsToRun)));
            isRunning = false;
        }
	}

	public void ResetClock() {
		secondsElapsed = 0;
        isRunning = true;
	}
}
