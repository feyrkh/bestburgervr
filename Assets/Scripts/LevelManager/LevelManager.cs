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

    public void StartLevel()
    {
        StartCoroutine("RunLevel");
    }

    public IEnumerator RunLevel()
    {
        WaitForSeconds pause = new WaitForSeconds(1f);
        waveRules.StartLevel(this);
        while (waveRules.IsRunning())
        {
            yield return pause;
        }
    }

    public void EndLevel()
    {
        waveRules.EndLevel();
    }
}

public abstract class NpcEnterStyle : MonoBehaviour
{
    public IEnumerator NpcEnter(Transform npc)
    {
        
        yield return null;
    }
}

public abstract class NpcExitStyle : MonoBehaviour
{

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
    }

    protected virtual void OnEndLevel()
    {
    }

    public virtual bool IsRunning()
    {
        return running;
    }

    public abstract void SpawnWave();
    public abstract void NpcServed(NpcOrder npc);
}

public abstract class OrderRules : MonoBehaviour
{
    public abstract void GenerateOrder(NpcOrder npc);
}

public class LevelSettings
{

}