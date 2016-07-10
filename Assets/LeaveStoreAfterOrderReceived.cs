using UnityEngine;
using System.Collections;

public class LeaveStoreAfterOrderReceived : MonoBehaviour {

    public void OnTimeToLeave()
    {
        StartCoroutine("LeaveAndDestroy");
    }

    public IEnumerator LeaveAndDestroy() { 
        yield return MoveUtil.MoveOverSeconds(gameObject, gameObject.transform.position + new Vector3(5, 0, 0), 3);
        Destroy(gameObject);
    }
}
