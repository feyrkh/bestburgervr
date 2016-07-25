using UnityEngine;
using System.Collections;

public class LeverListener : MonoBehaviour {
    public NewtonVR.NVRLever lever;


    // Update is called once per frame
    public virtual void  Update () {
        if (lever.LeverEngaged) OnLeverEngaged(lever);
	}

    public virtual void OnLeverEngaged(NewtonVR.NVRLever lever)
    {
        Debug.Log("Lever engaged!",this);
    }
}
