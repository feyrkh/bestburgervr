using UnityEngine;
using System.Collections;
using NewtonVR;

public class Sticky : MonoBehaviour {
    private bool canAttach = true;
    private NVRInteractableItem interactable;
    private FixedJoint myJoint;
    private static int flairStickyLayer = 0;

    public void Start()
    {
        interactable = GetComponentInParent<NewtonVR.NVRInteractableItem>();
        if(flairStickyLayer == 0)
        {
            flairStickyLayer = LayerMask.NameToLayer("flairSticky");
        }
    }

    public void OnBeginInteraction(NVRHand hand)
    {
        if(myJoint != null) { 
            myJoint.connectedBody.SendMessageUpwards("OnStickyDetached", transform);
            Destroy(myJoint);
            myJoint = null;
        }
        canAttach = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!canAttach) return;
        if (!interactable.IsAttached) return;
        if ((collider.gameObject.layer & flairStickyLayer) == 0) return;
        Debug.Log("Sticky touched something, attaching my parent", this);
        Transform touchedTransform = collider.transform;
        Rigidbody touchedBody = collider.GetComponent<Rigidbody>();
        if (touchedBody == null)
        {
            touchedBody = collider.GetComponentInParent<Rigidbody>();
        }
        if (touchedBody == null)
        {
            Debug.Log("Touched item doesn't have a rigidbody", touchedTransform);
            return;
        }
        Debug.Log("sticky attach touched: ", touchedTransform);
        AttachToBody(touchedBody);
        touchedBody.SendMessageUpwards("OnStickyAttached", transform);

        /*
        NewtonVR.NVRInteractable interactable = parentTransform.GetComponent<NewtonVR.NVRInteractable>();
        if(interactable)
        {
            interactable.ForceDetach();
            NewtonVR.NVRInteractables.Deregister(interactable);
        }
        Collider parentCollider = parentTransform.GetComponent<Collider>();
        Destroy(parentCollider);
        Destroy(this.gameObject);
        */
    }

    public void AttachToBody(Rigidbody attachingTo)
    {
        Transform parentTransform = transform.parent;
        if (parentTransform == null) parentTransform = transform;
        Debug.Log("sticky attach parent: ", parentTransform);
        myJoint = parentTransform.gameObject.AddComponent<FixedJoint>();
        FixedJoint connector = parentTransform.GetComponent<FixedJoint>();
        connector.connectedBody = attachingTo;
        canAttach = false;
    }
}
