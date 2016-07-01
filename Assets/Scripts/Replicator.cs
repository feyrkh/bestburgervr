using UnityEngine;
using System.Collections.Generic;
using NewtonVR;

public class Replicator : LeverListener {
    public Transform replicationArea;
    public Transform replicationOutput;

    public bool triggerReplication;

    public virtual void Update()
    {
        base.Update();
        if(triggerReplication)
        {
            OnLeverEngaged(null);
            triggerReplication = false;
        }
    }

    public override void OnLeverEngaged(NVRLever lever)
    {
        base.OnLeverEngaged(lever);
        HashSet<GameObject> cloneList = new HashSet<GameObject>();
        Debug.Log("Replicator engaged");
        Collider[] toReplicate = Physics.OverlapBox(replicationArea.transform.position, replicationArea.transform.lossyScale / 2f);
        Debug.Log("Found "+toReplicate.Length+" items to replicate");
        foreach (var item in toReplicate)
        {
            if (item.tag == "No Replication")
            {
                Debug.Log("Can't replicate this: " + item);
                continue;
            }
            cloneList.Add(item.gameObject);
        }

        foreach(var item in cloneList) { 
            var targetPoint = replicationOutput.TransformPoint(Vector3.zero);
            var adjustmentFromTarget = replicationArea.transform.position - item.transform.position;
            GameObject replicated = (GameObject)Instantiate(item,targetPoint-adjustmentFromTarget, item.transform.rotation);
            Debug.Log("Replicated "+replicated+" to " + replicated.transform.position +" using offset "+adjustmentFromTarget+" and target point "+targetPoint, replicated);
        }
    }

}
