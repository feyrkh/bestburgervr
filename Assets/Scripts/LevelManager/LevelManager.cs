﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager>
{
    protected LevelManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    public LevelSettings settings;
    public WaveRules waveRules;
    public OrderRules orderRules;
    public BurgerScorer burgerScorer;
    public Transform startPosition;
    public Transform orderPosition;
    public float coinCount = 0;
    public TipJar tipJar;
    public GameObject levelPrefab;
    public string menuLevel = null;
    public int currentlyLoadedSaveFile = 0;
    public bool wearingHat = false;

    public void Awake()
    {
        gameObject.SendMessage("OnLevelWasLoaded");
    }

    public SaveHatListEntry GetCurrentlyLoadedSaveFile()
    {
        return SaveHatShelf.GetSaveFile(currentlyLoadedSaveFile);
    }

    public bool WearHat(SaveHat hat)
    {
        if(!wearingHat || hat.saveFileId == currentlyLoadedSaveFile)
        {
            currentlyLoadedSaveFile = hat.saveFileId;
            wearingHat = true;
            return true;
        }
        return false;
    }

    public void RemoveHat(SaveHat hat)
    {
        if (hat.saveFileId == currentlyLoadedSaveFile)
        {
            wearingHat = false;
        }
    }

    public void OnLevelWasLoaded()
    {
        Debug.Log("Loaded level, spawning "+coinCount+" coins now", this);
        tipJar = FindObjectOfType<TipJar>();
        tipJar.SpawnCoins(coinCount);
        if (levelPrefab) {
            Debug.Log("Starting level with prefab", levelPrefab);
            StartLevel(levelPrefab);
            levelPrefab = null;
        }
    }

    public static void BroadcastAll(string fun, System.Object msg)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void ChangeScene(string sceneName, bool showGrid, float fadeOutTime)
    {
        BroadcastAll("OnSceneChanging", null);
        SteamVR_LoadLevel.Begin(sceneName, showGrid, fadeOutTime);
    }

    public void CountCoins()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        Debug.Log("Found coin count: " + coinCount);
        coinCount = 0;
        foreach (Coin coin in coins)
        {
            coinCount += coin.coinValue;
        }
        if(currentlyLoadedSaveFile > 0)
        {
            SaveHatShelf.GetSaveFile(currentlyLoadedSaveFile).coins = coinCount;
            SaveHatShelf.Save();
        }
    }

    public void StartLevel()
    {
        if(waveRules != null && waveRules.IsRunning())
        {
            Debug.LogError("Level is already running, ignoring");
            return;
        }
        startPosition = GameObject.Find("npcStartPosition").transform;
        orderPosition = GameObject.Find("npcOrderPosition").transform;
        StartCoroutine("RunLevel");
    }

    public void StartLevel(GameObject levelPrefab)
    {
        Debug.Log("Starting level");
        CountCoins();
        if (waveRules != null && waveRules.IsRunning())
        {
            Debug.LogError("Level is already running, ignoring");
            return;
        }
        waveRules = levelPrefab.GetComponent<WaveRules>();
        orderRules = levelPrefab.GetComponent<OrderRules>();
        burgerScorer = levelPrefab.GetComponent<BurgerScorer>();
        StartLevel();
    }

    public IEnumerator RunLevel()
    {
        Debug.Log("Starting a new level!");
        WaitForSeconds pause = new WaitForSeconds(1f);
        waveRules.StartLevel(this);
        while (waveRules.IsRunning())
        {
            yield return pause;
        }
        Debug.Log("All waves ended, level is over!");
        CountCoins();
    }

    public bool IsRunning()
    {
        if (waveRules == null) return false;
        return waveRules.IsRunning();
    }

    public void EndLevel()
    {
        waveRules.EndLevel();
    }
}

public class NpcEnterStyle : MonoBehaviour
{
    public virtual void StopMovement()
    {
        StopCoroutine("NpcEnter");
    }

    public virtual IEnumerator NpcEnter()
    {
        transform.position = LevelManager.Instance.orderPosition.position;
        yield return null;
    }
}

public class NpcExitStyle : MonoBehaviour
{
    public virtual void StopMovement()
    {
        StopCoroutine("NpcExit");
    }

    public virtual IEnumerator NpcExit()
    {
        Destroy(gameObject);
        yield return null;
    }
}

public abstract class WaveRules : MonoBehaviour
{
    protected LevelManager levelManager;
    public bool running;

    public void StartLevel(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        this.running = true;
        OnStartLevel();
    }

    public void EndLevel()
    {
        running = false;
        OnEndLevel();
    }

    protected virtual void OnStartLevel()
    {
        StartCoroutine("SpawnWave");
    }

    protected virtual void OnEndLevel()
    {
        LevelManager.Instance.ChangeScene(LevelManager.Instance.menuLevel, false, 2);
    }

    public virtual bool IsRunning()
    {
        return running;
    }

    public abstract IEnumerator SpawnWave();
    public abstract IEnumerator NpcServed(NpcOrder npcOrder, CompletedBurger completedBurger);
}

public abstract class OrderRules : MonoBehaviour
{
    public virtual int CalculateCurrentOrderComplexity()
    {
        return Mathf.RoundToInt(1.5f + LevelManager.Instance.settings.difficultyLevel * 0.55f + Random.Range(-0.6f, 0.6f));
    }

    public abstract void GenerateOrder(NpcOrder npc);
}

public class LevelSettings
{
    public int gameModeIndex
    {
        get { return _gameModeIndex; }
        set
        {
            _gameModeIndex = value;
            Debug.Log("Setting _gameModeIndex to " + _gameModeIndex);
            if (_gameModeIndex < 0) _gameModeIndex = gameModeOptions.Length - 1;
            if (_gameModeIndex >= gameModeOptions.Length) _gameModeIndex = 0;

            Debug.Log("Actually set to " + _gameModeIndex);
        }
    }
    public int difficultyLevelIndex
    {
        get { return _difficultyLevelIndex; }
        set
        {
            _difficultyLevelIndex = value;
            if (_difficultyLevelIndex < 0) _difficultyLevelIndex = difficultyLevelOptions.Length - 1;
            if (_difficultyLevelIndex >= difficultyLevelOptions.Length) _difficultyLevelIndex = 0;
        }
    }

    public int difficultyLevel
    {
        get { return difficultyLevelOptions[_difficultyLevelIndex];  }
    }
    public int customerCountIndex
    {
        get { return _customerCountIndex; }
        set
        {
            _customerCountIndex = value;
            if (_customerCountIndex < 0) _customerCountIndex = customerCountOptions.Length - 1;
            if (_customerCountIndex >= customerCountOptions.Length) _customerCountIndex = 0;
        }
    }
    public int customerCount
    {
        get { return customerCountOptions[_customerCountIndex]; }
    }
    public string gameMode
    {
        get
        {
            return gameModeOptions[_gameModeIndex];
        }
    }

    public const string MODE_FAST_FOOD = "Fast Food";
    public const string MODE_EARLY_BIRD = "Early Bird Special";
    public const string MODE_LUNCH_RUSH = "Lunch Rush";
    public const string MODE_FRANCHISE = "Franchise";
    private int _gameModeIndex = 0;
    private int _customerCountIndex = 2;
    private int _difficultyLevelIndex = 0;  
      
    public string[] gameModeOptions = { MODE_FAST_FOOD, MODE_EARLY_BIRD, MODE_LUNCH_RUSH, MODE_FRANCHISE };
    public int[] customerCountOptions = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 9999 };
    public int[] difficultyLevelOptions = { 1, 2, 3, 4, 5 };
    public float bonusScaling {
        get
        {
            float minScaling = 0.6f;
            float maxScaling = 1.4f;
            float scaleStep = (maxScaling - minScaling) / (difficultyLevelOptions.Length-1);
            return minScaling + scaleStep * _difficultyLevelIndex;
        }
    }
}

public enum GameMode
{
    FastFood, EarlyBirdSpecial, LunchRush, Campaign
}