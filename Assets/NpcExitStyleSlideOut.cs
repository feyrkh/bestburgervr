using UnityEngine;
using System.Collections;

public class NpcExitStyleSlideOut : NpcExitStyle
{
    Vector3 distance = new Vector3(10f, 0, 0);
    public override IEnumerator NpcExit(Transform npc)
    {
        Debug.Log("Moving npc to exit", npc);
        npc.position = LevelManager.Instance.orderPosition.position;
        yield return MoveUtil.MoveOverSeconds(npc.gameObject, LevelManager.Instance.startPosition.position, 1);
        Debug.Log("Finished moving to exit, destroying npc", npc);
        Destroy(npc.gameObject);
    }
}
