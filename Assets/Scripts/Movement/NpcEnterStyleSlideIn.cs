using UnityEngine;
using System.Collections;

public class NpcEnterStyleSlideIn : NpcEnterStyle {
    public override IEnumerator NpcEnter()
    {
        transform.position = LevelManager.Instance.startPosition.position;
        yield return MoveUtil.MoveOverSeconds(transform.gameObject, LevelManager.Instance.orderPosition.position, 1);
    }
}
