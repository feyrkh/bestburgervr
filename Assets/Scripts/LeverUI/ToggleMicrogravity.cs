using UnityEngine;
using System.Collections;

public class ToggleMicrogravity : MonoBehaviour {
    public NewtonVR.NVRSwitch connectedSwitch = null;

    public void OnSwitchEnabled(string switchName)
    {
        Physics.gravity = new Vector3(0, -0.15f, 0);
    }
    public void OnSwitchDisabled(string switchName)
    {
        Physics.gravity = new Vector3(0, -9.8f, 0);
    }

    public void SetShortHumanMode(bool enabled)
    {
        Debug.Log("Short human mode: " + enabled);
        var player = FindObjectOfType<NewtonVR.NVRPlayer>();

        if (enabled)
        {
            player.transform.localScale = new Vector3(1, 1.5f, 1);  
        } else
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
