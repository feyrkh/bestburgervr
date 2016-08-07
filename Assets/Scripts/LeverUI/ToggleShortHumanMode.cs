using UnityEngine;
using System.Collections;

public class ToggleShortHumanMode : MonoBehaviour {
    public NewtonVR.NVRSwitch connectedSwitch = null;
    public float secondsToChange = 2;
    public float targetSize = 1.5f;
    bool isEnabled = false;

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
        if (this.isEnabled == enabled) return;
        this.isEnabled = enabled;
        Debug.Log("Short human mode: " + enabled);
        var player = FindObjectOfType<NewtonVR.NVRPlayer>();

        if (enabled)
        {
            StartCoroutine("GrowTo", targetSize);
        } else
        {
            StartCoroutine("GrowTo", 1);
        }
    }

    public IEnumerator GrowTo(float target)
    {
        var player = FindObjectOfType<NewtonVR.NVRPlayer>();
        Vector3 start = player.transform.localScale;
        Vector3 end = new Vector3(target, target, target);
        float secondsElapsed = 0;
        while(secondsElapsed < secondsToChange)
        {
            secondsElapsed += Time.deltaTime;
            player.transform.localScale = Vector3.Lerp(start, end, secondsElapsed / secondsToChange);
            yield return new WaitForEndOfFrame();
        }
    }
}
