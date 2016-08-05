using UnityEngine;
using System.Collections;

public class SetQualityLevel : MonoBehaviour {

    public string[] qualitySettingNames;
    public int currentQuality;
    private int previousQuality;

    public void Awake()
    {
        qualitySettingNames = QualitySettings.names;
        currentQuality = QualitySettings.GetQualityLevel();
        previousQuality = QualitySettings.GetQualityLevel();
    }

    public void Update()
    {
        if(currentQuality != previousQuality)
        {
            previousQuality = currentQuality;
            QualitySettings.SetQualityLevel(currentQuality, true);
        }
    }
}
