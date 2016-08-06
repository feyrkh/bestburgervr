using UnityEngine;
using System.Collections;

public class MysteryHatFlair : MonoBehaviour {
    public void OnVendComplete()
    {
        GetComponent<HatFlair>().ChooseRandomFlairIcon();
        GetComponent<HatFlair>().ApplyFlairSettings();
        Destroy(this);
    }
}
