using UnityEngine;
using System.Collections;

public class NpcEnterStyleEarlyBird : NpcEnterStyle {
    public override IEnumerator NpcEnter()
    {
        Vector3 offset = new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.1f, 0.1f));
        float timeOffset = Random.Range(0, 6f);
        transform.position = LevelManager.Instance.startPosition.position;
        yield return MoveUtil.MoveOverSeconds(transform.gameObject, LevelManager.Instance.orderPosition.position+offset, 8+timeOffset);
    }
}
