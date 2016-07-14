using UnityEngine;
using System.Collections;

public class Sticky : MonoBehaviour {
    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Sticky touched something, attaching my parent and then destroying myself");
        Transform parentTransform = transform.parent;
        Transform touchedTransform = collider.transform;
        Rigidbody touchedBody = collider.GetComponent<Rigidbody>();
        if (touchedBody == null)
        {
            Debug.Log("Touched item doesn't have a rigidbody", touchedTransform);
            return;
        }
        Debug.Log("sticky attach parent: ", parentTransform);
        Debug.Log("sticky attach touched: ", touchedTransform);
        parentTransform.gameObject.AddComponent<FixedJoint>();
        FixedJoint connector = parentTransform.GetComponent<FixedJoint>();
        connector.connectedBody = touchedBody;

        NewtonVR.NVRInteractable interactable = parentTransform.GetComponent<NewtonVR.NVRInteractable>();
        if(interactable)
        {
            interactable.ForceDetach();
            NewtonVR.NVRInteractables.Deregister(interactable);
        }
        Collider parentCollider = parentTransform.GetComponent<Collider>();
        Destroy(parentCollider);
        Destroy(this.gameObject);
    }
}
