using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SetLevelRules : LeverListener {
    public string rulesetName = "unknown";
    public NpcEnterStyle npcEnterStylePrefab;
    public NpcExitStyle npcExitStylePrefab;
    public WaveRules waveRulesPrefab;
    public OrderRules orderRulesPrefab;
    public LevelSettings levelSettingsPrefab;

    Hashtable instances = new Hashtable();

    public void OnLeverEngaged()
    {
        OnLeverEngaged(null);
    }

    public override void OnLeverEngaged(NewtonVR.NVRLever lever)
    {
        if (LevelManager.Instance.npcEnterStyle != null) Destroy(LevelManager.Instance.npcEnterStyle.gameObject);
        if(LevelManager.Instance.npcExitStyle != null) Destroy(LevelManager.Instance.npcExitStyle.gameObject);
        if(LevelManager.Instance.orderRules != null) Destroy(LevelManager.Instance.orderRules.gameObject);
        if(LevelManager.Instance.waveRules != null) Destroy(LevelManager.Instance.waveRules.gameObject);
        if(LevelManager.Instance.settings != null) Destroy(LevelManager.Instance.settings.gameObject);

        if(LevelManager.Instance.IsRunning())
        {
            LevelManager.Instance.EndLevel();
        }

        LevelManager.Instance.npcEnterStyle = BuildFromPrefab(npcEnterStylePrefab);
        LevelManager.Instance.npcExitStyle = BuildFromPrefab(npcExitStylePrefab);
        LevelManager.Instance.orderRules = BuildFromPrefab(orderRulesPrefab);
        LevelManager.Instance.waveRules = BuildFromPrefab(waveRulesPrefab);
        LevelManager.Instance.settings = BuildFromPrefab(levelSettingsPrefab);

        LevelManager.Instance.StartLevel();
    }

    private T BuildFromPrefab<T>(T prefab) where T: UnityEngine.MonoBehaviour
    {
        if (!instances.ContainsKey(prefab.gameObject))
        {
            instances.Add(prefab.gameObject, Instantiate(prefab.gameObject));
        }
        return ((GameObject)instances[prefab.gameObject]).GetComponent<T>();
    }
}
