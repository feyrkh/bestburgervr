using UnityEngine;
using System.Collections;

public class MoveUtil  {

    public static IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.localPosition;
        Vector3 startingRot = objectToMove.transform.localEulerAngles;

        while (elapsedTime < seconds)
        {
            objectToMove.transform.localPosition = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.localPosition = end;
        //objectToMove.transform.localEulerAngles = Vector3.zero;
    }

}
