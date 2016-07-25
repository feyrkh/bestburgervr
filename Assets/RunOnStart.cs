using UnityEngine;
using System.Collections;

public class RunOnStart : MonoBehaviour {
    public float secondsToDelay = 5;
    public string messageToSend = "OnStart";

	// Use this for initialization
	void Start () {
        StartCoroutine("RunAfterDelay");
	}

    public IEnumerator RunAfterDelay()
    {
        yield return new WaitForSeconds(secondsToDelay);
        SendMessage(messageToSend);
    }
	
}
