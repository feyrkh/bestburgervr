using UnityEngine;
using System.Collections;

public class NpcEnterStyleSlideIn : NpcEnterStyle {
    public override IEnumerator NpcEnter(Transform npc)
    {
        npc.position = LevelManager.Instance.startPosition.position;
        Debug.Log("Sliding npc in", npc);
        yield return MoveUtil.MoveOverSeconds(npc.gameObject, LevelManager.Instance.orderPosition.position, 1);
        Debug.Log("Done sliding npc in", npc);
    }
}
