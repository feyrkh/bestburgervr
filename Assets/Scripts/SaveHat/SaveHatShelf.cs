using UnityEngine;
using System.Collections.Generic;
using System;

public class SaveHatShelf : MonoBehaviour {
    public int shelfIndex = 0;
    public int hatCount = 5;
    public float lowHueRange = 0;
    public float highHueRange = 1;
    public SaveHat hatPrefab;

    SaveHatData saveData = new SaveHatData();

    public void Awake()
    {
        Load();
    }

    public void Save()
    {
        SaveLoad.Save(saveData, GetSaveFile());
    }

    private string GetSaveFile()
    {
        return "shelf" + shelfIndex + ".hat";
    }

    public void Load()
    {
        saveData = SaveLoad.Load<SaveHatData>(GetSaveFile());
        if (saveData == null)
        {
            saveData = new SaveHatData();
        }
        BuildHats();
    }

    private void BuildHats()
    {
        int hatId = 100 * shelfIndex;
        float shelfWidth = GetComponent<Collider>().bounds.extents.z * 2;
        float spacePerHat = shelfWidth / hatCount;
        float yPos = GetComponent<Collider>().bounds.extents.y;
        for (int i=0;i<hatCount;i++)
        {
            SaveHatListEntry curData = null;
            if(saveData.saveHats.ContainsKey(hatId+i)) curData = saveData.saveHats[hatId + i];
            if(curData == null)
            {
                curData = new SaveHatListEntry();
                float hue = lowHueRange + ((highHueRange - lowHueRange) / hatCount) * i;
                curData.color = Color.HSVToRGB(hue, 1f, 1f);
                curData.flair = new List<SaveHatFlairEntry>();
                saveData.saveHats[hatId + i] = curData;
                Debug.Log("Creating new hat. hue=" + hue + ", id=" + (hatId + i));
            }
            Vector3 position = new Vector3(0, yPos, spacePerHat / 2 + spacePerHat * i - (shelfWidth/2));
            SaveHat curHat = (SaveHat)Instantiate(hatPrefab, position + transform.position, Quaternion.identity);
            Debug.Log("Instantiated hat", curHat);
            curHat.SetColor(curData.color);
        }
    }
}

[Serializable]
class SaveHatData
{
    public Dictionary<int, SaveHatListEntry> saveHats = new Dictionary<int, SaveHatListEntry>();
}

[Serializable]
class SaveHatListEntry
{
    public Color color;
    public List<SaveHatFlairEntry> flair; 
}

[Serializable]
class SaveHatFlairEntry
{
    public Transform position;
    public string icon;
    public float size;
}