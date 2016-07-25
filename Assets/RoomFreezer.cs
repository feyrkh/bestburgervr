using UnityEngine;
using System.Collections;

public class RoomFreezer : MonoBehaviour
{
    public enum MovementFunction
    {
        Nonlinear,
        LinearDirect
    }

    public MovementFunction movementFunction = MovementFunction.LinearDirect;
    public bool shouldFreezeMovement = false;
    public bool shouldFreezeRotation = false;
    public Transform debugTransform;

    [HideInInspector]
    public Vector3 relativeMovementOfCameraRig = new Vector3();

    protected Transform movementTransform;
    protected Transform cameraRig;
    protected Vector3 headCirclePosition;
    protected Vector3 lastPosition;
    protected Vector3 lastMovement;
    protected float lastRotation;

    private void Start()
    {
        if (movementTransform == null)
        {
            if (VRTK.DeviceFinder.HeadsetTransform() != null)
            {
                movementTransform = VRTK.DeviceFinder.HeadsetTransform();
            }
            else
            {
                Debug.LogWarning("This 'RoomFreezer' script needs a movementTransform to work.The default 'SteamVR_Camera' or 'SteamVR_GameView' was not found.");
            }
        }
        cameraRig = GameObject.FindObjectOfType<SteamVR_PlayArea>().gameObject.transform;
        //headCirclePosition = new Vector3(movementTransform.localPosition.x, 0, movementTransform.localPosition.z);
        MoveHeadCircleNonLinearDrift();
        lastPosition = movementTransform.localPosition;
        lastRotation = movementTransform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        switch (movementFunction)
        {
            case MovementFunction.Nonlinear:
                MoveHeadCircleNonLinearDrift();
                break;
            case MovementFunction.LinearDirect:
                MoveHeadCircle();
                break;
            default:
                break;
        }
        lastPosition = movementTransform.localPosition;
        lastRotation = movementTransform.localRotation.eulerAngles.y;
    }

    private void Move(Vector3 movement)
    {
        headCirclePosition += movement;
        if (debugTransform)
        {
            debugTransform.localPosition = headCirclePosition;
        }
        cameraRig.localPosition += movement;
        relativeMovementOfCameraRig += movement;
    }

    private void MoveHeadCircle()
    {
        //Get the movement of the head relative to the headCircle.
        var circleCenterToHead = new Vector3(movementTransform.localPosition.x - headCirclePosition.x, 0, movementTransform.localPosition.z - headCirclePosition.z);

        if (shouldFreezeRotation)
        {
            float movementRotationDiff = movementTransform.localRotation.eulerAngles.y - lastRotation;
            Vector3 rotation = transform.localRotation.eulerAngles;
            rotation.y -= movementRotationDiff;
            transform.RotateAround(movementTransform.position, Vector3.up, -movementRotationDiff);
            //            transform.localRotation = Quaternion.Euler(rotation);
        }

        //Get the direction of the head movement.
        UpdateLastMovement();
        if (shouldFreezeMovement)
        {
            // Invert the headset move
            Move(-lastMovement);
        }
    }

    private void MoveHeadCircleNonLinearDrift()
    {
        var movement = new Vector3(movementTransform.localPosition.x - headCirclePosition.x, 0, movementTransform.localPosition.z - headCirclePosition.z);

            var deltaMovement = movement.normalized * (movement.magnitude);
            Move(deltaMovement);
        
    }

    private void UpdateLastMovement()
    {
        //Save the last movement
        lastMovement = movementTransform.localPosition - lastPosition;
        lastMovement.y = 0;
    }
}