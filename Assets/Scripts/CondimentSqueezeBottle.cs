using UnityEngine;
using System.Collections;

public class CondimentSqueezeBottle : NewtonVR.NVRInteractableItem {
    public GameObject bulletPrefab;
    public Vector3 bulletForce = new Vector3(0, 1, 0);
    public Transform firePoint;
    
    public override void UseButtonDown()
    {
        base.UseButtonDown();

        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        bullet.GetComponent<Rigidbody>().AddRelativeForce(bulletForce);

        AttachedHand.TriggerHapticPulse(100, Valve.VR.EVRButtonId.k_EButton_Axis0);
    }

}
