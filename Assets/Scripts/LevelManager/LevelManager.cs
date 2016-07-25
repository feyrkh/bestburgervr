using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager>
{
    protected LevelManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    public LevelSettings settings;
    public WaveRules waveRules;
    public OrderRules orderRules;
    public NpcEnterStyle npcEnterStyle;
    public NpcExitStyle npcExitStyle;
    public Transform startPosition;
    public Transform orderPosition;

    public void StartLevel()
    {
        startPosition = GameObject.Find("npcStartPosition").transform;
        orderPosition = GameObject.Find("npcOrderPosition").transform;
        StartCoroutine("RunLevel");
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
    public virtual IEnumerator NpcEnter(Transform npc)
    {
        npc.position = LevelManager.Instance.orderPosition.position;
        yield return null;
    }
}

public class NpcExitStyle : MonoBehaviour
{
    public virtual IEnumerator NpcExit(Transform npc)
    {
        Debug.Log("Destroying npc", npc);
        Destroy(npc.gameObject);
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
        SpawnWave();
    }

    protected virtual void OnEndLevel()
    {
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
    public abstract void GenerateOrder(NpcOrder npc);
}

public class LevelSettings : MonoBehaviour
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

    private int _gameModeIndex = 0;
    private int _customerCountIndex = 2;
    private int _difficultyLevelIndex = 0;  
      
    public string[] gameModeOptions = { "Fast Food", "Early Bird Special", "Lunch Rush", "Franchise" };
    public int[] customerCountOptions = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 9999 };
    public int[] difficultyLevelOptions = { 1, 2, 3, 4, 5 };
}

public enum GameMode
{
    FastFood, EarlyBirdSpecial, LunchRush, Campaign
}