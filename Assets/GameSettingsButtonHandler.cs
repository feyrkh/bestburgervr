using UnityEngine;
using System.Collections;

public class GameSettingsButtonHandler : MonoBehaviour {
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
}
