using UnityEngine;
using System.Collections;

public class GameSettingsButtonHandler : LeverListener {
    public WaveRules fastFoodLevelPrefab;
    public WaveRules earlyBirdLevelPrefab;
    public WaveRules lunchRushLevelPrefab;
    public WaveRules campaignLevelPrefab;

    public VRTK.VRTK_ObjectTooltip gameModeLabel;
    public VRTK.VRTK_ObjectTooltip customerCountLabel;
    public VRTK.VRTK_ObjectTooltip difficultyLevelLabel;

    public void Awake()
    {
        if(LevelManager.Instance.settings == null)
        {
            Debug.Log("Creating new settings for LevelManager");
            LevelManager.Instance.settings = new LevelSettings();
        }
        gameModeLabel = GameObject.Find("gameModeLabel").GetComponent<VRTK.VRTK_ObjectTooltip>();
        customerCountLabel = GameObject.Find("customerCountLabel").GetComponent<VRTK.VRTK_ObjectTooltip>();
        difficultyLevelLabel = GameObject.Find("difficultyLevelLabel").GetComponent<VRTK.VRTK_ObjectTooltip>();
        UpdateLabels();
    }

    public void OnButtonPushed(string buttonName)
    {
        Debug.Log("Button pushed: " + buttonName);

        switch(buttonName)
        {
            case "gameModeDown": LevelManager.Instance.settings.gameModeIndex -= 1; break;
            case "gameModeUp": LevelManager.Instance.settings.gameModeIndex += 1; break;
            case "difficultyLevelDown": LevelManager.Instance.settings.difficultyLevelIndex--; break;
            case "difficultyLevelUp": LevelManager.Instance.settings.difficultyLevelIndex++; break;
            case "customerCountDown": LevelManager.Instance.settings.customerCountIndex--; break;
            case "customerCountUp": LevelManager.Instance.settings.customerCountIndex++; break;
        }

        UpdateLabels();
    }

    public void UpdateLabels()
    {
        gameModeLabel.displayText = "Mode: " + LevelManager.Instance.settings.gameMode;
        customerCountLabel.displayText = "Customers: " + LevelManager.Instance.settings.customerCount;
        difficultyLevelLabel.displayText = "Difficulty: " + LevelManager.Instance.settings.difficultyLevel;
        gameModeLabel.Reset();
        customerCountLabel.Reset();
        difficultyLevelLabel.Reset();
    }

    public override void OnLeverEngaged(NewtonVR.NVRLever lever)
    {
        Debug.Log("Starting level!", this);
        LevelManager.Instance.menuLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        WaveRules prefab = null;
        switch (LevelManager.Instance.settings.gameMode)
        {
            case LevelSettings.MODE_FAST_FOOD:
                prefab = Instantiate(fastFoodLevelPrefab);
                break;
            case LevelSettings.MODE_EARLY_BIRD:
                prefab = Instantiate(earlyBirdLevelPrefab);
                break;
            case LevelSettings.MODE_LUNCH_RUSH:
                prefab = Instantiate(lunchRushLevelPrefab);
                break;
            case LevelSettings.MODE_FRANCHISE:
                prefab = Instantiate(campaignLevelPrefab);
                break;
        }
        if(prefab == null)
        {
            Debug.LogError("Bad game mode name, no prefab: " + LevelManager.Instance.settings.gameMode);
        }
        DontDestroyOnLoad(prefab);
        if(LevelManager.Instance.levelPrefab != null)
        {
            Destroy(LevelManager.Instance.levelPrefab);
        }
        LevelManager.Instance.levelPrefab = prefab.gameObject;
        Debug.Log("LevelManager has settings: " + LevelManager.Instance.settings);
        SteamVR_LoadLevel.Begin("Main");
    }
}
