using UnityEngine;
using System.Collections;
using VRTK;

public class RoomFreezerController : MonoBehaviour
{
    protected RoomFreezer roomExtender;
    public bool freezeRotationOnPadPress = false;


    // Use this for initialization
    private void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("VRTK_RoomExtender_ControllerExample is required to be attached to a SteamVR Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }
        if (FindObjectOfType<RoomFreezer>() == null)
        {
            Debug.LogError("RoomFreezer is required to be attached to the CameraRig that has the RoomFreezerController script attached to it");
            return;
        }
        roomExtender = FindObjectOfType<RoomFreezer>();
        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
        GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        //roomExtender.additionalMovementMultiplier = e.touchpadAxis.magnitude * 5 > 1 ? e.touchpadAxis.magnitude * 5 : 1;
            EnableAdditionalMovement(e);
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
            DisableAdditionalMovement();
    }

    private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        switch (roomExtender.movementFunction)
        {
            case RoomFreezer.MovementFunction.Nonlinear:
                roomExtender.movementFunction = RoomFreezer.MovementFunction.LinearDirect;
                break;
            case RoomFreezer.MovementFunction.LinearDirect:
                roomExtender.movementFunction = RoomFreezer.MovementFunction.Nonlinear;
                break;
            default:
                break;
        }
    }

    private void EnableAdditionalMovement(ControllerInteractionEventArgs e)
    {
        if(Mathf.Abs(e.touchpadAxis.x) > 0.8f)
        {
            // freeze rotation
            roomExtender.shouldFreezeRotation = true;
        }
        if (Mathf.Abs(e.touchpadAxis.y) > 0.8f) 
        {
            // freeze movement
            roomExtender.shouldFreezeMovement = true;
        }
    }

    private void DisableAdditionalMovement()
    {
        roomExtender.shouldFreezeRotation = false;
        roomExtender.shouldFreezeMovement = false;
    }
}