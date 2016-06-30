using UnityEngine;
using System.Collections;
// Expects to find a SteamVR_Teleporter and SteamVR_LaserPointer on the parent
// When the D pad is pressed, enables laser pointer and teleporting with trigger pull
// When the D pad is released, disables teleporting
public class TeleportManager : MonoBehaviour {
    public SteamVR_LaserPointer laser;
    SteamVR_Teleporter teleporter;

    // Use this for initialization
    void Start() {
        teleporter = GetComponent<SteamVR_Teleporter>();
        laser.active = false;
        laser.gameObject.SetActive(false);

        var trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = gameObject.AddComponent<SteamVR_TrackedController>();
        }

        trackedController.PadClicked += new ClickedEventHandler(DoPadClick);
        trackedController.PadUnclicked += new ClickedEventHandler(DoPadUnclick);
    }

    void DoPadClick(object sender, ClickedEventArgs e) {
        laser.active = true;
        laser.gameObject.SetActive(true);
        teleporter.teleportOnClick = true;
    }

    void DoPadUnclick(object sender, ClickedEventArgs e)
    {
        laser.active = false;
        laser.gameObject.SetActive(false);
        teleporter.teleportOnClick = false;
    }


    // Update is called once per frame
    void Update () {
        if (laser.active)
        {
            laser.transform.position = transform.position;
            laser.transform.rotation = transform.rotation;
        }
	}
}
