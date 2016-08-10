using UnityEngine;
using System.Collections;

public class NpcExitStyleSlideOut : NpcExitStyle
{
    public float secondsToMove = 1f;
    Vector3 distance = new Vector3(10f, 0, 0);
    public override IEnumerator NpcExit()
    {
        //transform.position = LevelManager.Instance.orderPosition.position;
        yield return MoveUtil.MoveOverSeconds(transform.gameObject, LevelManager.Instance.startPosition.position, secondsToMove);
        Destroy(gameObject.gameObject);
    }
}
