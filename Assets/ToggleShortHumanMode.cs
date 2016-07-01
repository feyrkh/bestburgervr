using UnityEngine;
using System.Collections;

public class ToggleShortHumanMode : MonoBehaviour {
    public NewtonVR.NVRSwitch connectedSwitch;

    public void OnSwitchEnabled(string switchName)
    {
        if (switchName == connectedSwitch.switchName)
        {
            SetShortHumanMode(true);
        }
    }
    public void OnSwitchDisabled(string switchName)
    {
        if (switchName == connectedSwitch.switchName)
        {
            SetShortHumanMode(false);
        }
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
