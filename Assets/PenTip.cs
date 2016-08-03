using UnityEngine;
using System.Collections.Generic;
using NewtonVR;
using Vectrosity;

public class PenTip : MonoBehaviour {
    public GameObject penMarkPrefab;
    public float overlapRadius = 0.005f;
    float sqrOverlapRadius;
    Collider[] nearbyObjects = new Collider[10];
    private NVRInteractableItem interactable;
    RaycastHit hitInfo = new RaycastHit();
    

    Dictionary<GameObject, VectorLine> currentlyDrawing = new Dictionary<GameObject, VectorLine>();

    public void Start()
    {
        sqrOverlapRadius = overlapRadius * overlapRadius;
        interactable = GetComponentInParent<NewtonVR.NVRInteractableItem>();
    }

    /*
    public void OnTriggerEnter(Collider collision)
    {
        // if (!interactable.IsAttached) return;
        Vector3 point = collision.transform.InverseTransformPoint(collision.ClosestPointOnBounds(this.transform.position));

        collision.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.up)), out hitInfo, 0.01f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        Quaternion oldRot = camera.transform.rotation;
        camera.transform.rotation = rot;
        
        VectorLine line = VectorLine.SetLine3D(Color.black, point, point);
        line.drawTransform = collision.transform;
        line.lineType = LineType.Continuous;
        line.SetWidth(1f);
        currentlyDrawing[collision.gameObject] = line;
        Debug.Log("Started drawing line", collision);
        camera.transform.rotation = oldRot;
    }

    public void OnTriggerLeave(Collider collision)
    {
        GameObject key = collision.gameObject;
        if(currentlyDrawing.ContainsKey(key))
            currentlyDrawing.Remove(key);
        Debug.Log("Stopped drawing line", collision);
    }

    public void OnTriggerStay(Collider collision)
    {
        GameObject key = collision.gameObject;
        if (!currentlyDrawing.ContainsKey(key)) return;
        VectorLine line = currentlyDrawing[key];
        Vector3 lastPoint = line.points3[line.points3.Count - 1];
        Vector3 newPoint = collision.transform.InverseTransformPoint(collision.ClosestPointOnBounds(this.transform.position));
        Vector3 distance = newPoint - lastPoint;
        if (distance.sqrMagnitude < sqrOverlapRadius) return;
        Debug.Log("Added new point to line");
        line.points3.Add(newPoint);
        line.MakeSpline(line.points3.ToArray());
    }*/
    
    public void OnTriggerStay(Collider collision)
    {
        //if (!interactable.IsAttached) return;
        Vector3 pos = collision.ClosestPointOnBounds(this.transform.position);
        int numberNearbyObjects = Physics.OverlapSphereNonAlloc(pos, overlapRadius, nearbyObjects);
        for(int i=0;i<numberNearbyObjects && i<nearbyObjects.Length;i++)
        {
            if(nearbyObjects[i].GetComponent<PenMark>() != null)
            {
                return;
            }
        }
        collision.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.up)), out hitInfo, 0.01f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        GameObject mark = (GameObject)Instantiate(penMarkPrefab, pos, rot);
        mark.transform.SetParent(collision.transform);
        Debug.Log("Making new mark", mark);
    }
}
