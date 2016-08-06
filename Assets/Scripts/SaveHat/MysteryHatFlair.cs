using UnityEngine;
using System.Collections;

public class MysteryHatFlair : MonoBehaviour {
    public void OnVend()
    {
        GetComponent<HatFlair>().ChooseRandomFlairIcon();
        GetComponent<HatFlair>().ApplyFlairSettings();
        Destroy(this);
    }
}
