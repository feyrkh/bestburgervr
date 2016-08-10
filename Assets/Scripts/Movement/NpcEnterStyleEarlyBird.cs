using UnityEngine;
using System.Collections;

public class NpcEnterStyleEarlyBird : NpcEnterStyle {
    public ClockTimer clockPrefab;
    public float failureSeconds = 6f;
    public float failureSecondsPerDifficultyLevel = 2f;

    public float travelSeconds = 10f;
    public float travelSecondsRandomMax = 25f;
    public float extraTravelSecondsPerDifficultyLevel = 3f;

    public override IEnumerator NpcEnter()
    {
        Vector3 offset = new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.1f, 0.1f));
        float timeOffset = Random.Range(0, travelSecondsRandomMax) + extraTravelSecondsPerDifficultyLevel * LevelManager.Instance.settings.difficultyLevel;
        transform.position = LevelManager.Instance.startPosition.position;
        yield return MoveUtil.MoveOverSeconds(transform.gameObject, LevelManager.Instance.orderPosition.position + offset, travelSeconds+timeOffset);
        ClockTimer clock = (ClockTimer)Instantiate(clockPrefab, transform);
        clock.transform.localPosition = new Vector3(0, 0.6f, 0);
        clock.name = "clock";
        clock.restart = false;
        clock.secondsToRun = failureSeconds + failureSecondsPerDifficultyLevel * LevelManager.Instance.settings.difficultyLevel;
        clock.ResetClock();
    }

    public void OnClockTimerElapsed(ClockTimer clock)
    {
        LevelManager.Instance.waveRules.EndLevel();
    }
}
