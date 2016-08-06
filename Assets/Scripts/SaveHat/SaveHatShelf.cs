using UnityEngine;
using System.Collections.Generic;
using System;

public class SaveHatShelf : MonoBehaviour {
    public int shelfIndex = 0;
    public int hatCount = 5;
    public float lowHueRange = 0;
    public float highHueRange = 1;
    public SaveHat hatPrefab;

    private static SaveHatData saveData = null;

    public HatFlair hatFlairPrefab;
    private static HatFlair globalHatFlairPrefab;

    public void Awake()
    {
        if(hatFlairPrefab != null) globalHatFlairPrefab = hatFlairPrefab;
        Load();
        BuildHats();
    }
    
    public static SaveHatListEntry GetSaveFile(int id)
    {
        return saveData.saveHats[id];
    }

    public static void Save()
    {
        SaveLoad.Save(saveData, GetSaveFile());
    }

    private static string GetSaveFile()
    {
        return "hat.shelves";
    }

    public static void Load()
    {
        if (saveData == null)
        {
            try
            {
                saveData = SaveLoad.Load<SaveHatData>(GetSaveFile());
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load save file" + e.Message);
            }
        }
        if (saveData == null)
        {
            saveData = new SaveHatData();
        }
    }

    private void BuildHats()
    {
        int hatId = 100 * shelfIndex;
        float shelfWidth = GetComponent<Collider>().bounds.extents.z * 2;
        float spacePerHat = shelfWidth / hatCount;
        float yPos = GetComponent<Collider>().bounds.extents.y;
        for (int i=0;i<hatCount;i++)
        {
            // If the player is already wearing the hat we're about to build, don't build it
            if (LevelManager.Instance.wearingHat && LevelManager.Instance.currentlyLoadedSaveFile == hatId + i) continue;
            SaveHatListEntry curData = null;
            if(saveData.saveHats.ContainsKey(hatId+i)) curData = saveData.saveHats[hatId + i];
            if(curData == null)
            {
                curData = new SaveHatListEntry();
                float hue = lowHueRange + ((highHueRange - lowHueRange) / hatCount) * i;
                Color color = Color.HSVToRGB(hue, 1f, 1f);
                curData.r = color.r;
                curData.g = color.g;
                curData.b = color.b;
                curData.flair = new List<SaveHatFlairEntry>();
                saveData.saveHats[hatId + i] = curData;
                Debug.Log("Creating new hat. hue=" + hue + ", id=" + (hatId + i));
            }
            Vector3 position = new Vector3(0, yPos, spacePerHat / 2 + spacePerHat * i - (shelfWidth/2));
            SaveHat curHat = (SaveHat)Instantiate(hatPrefab, position + transform.position, Quaternion.Euler(0, -160, 0));
            Debug.Log("Instantiated hat", curHat);
            curHat.SetColor(new Color(curData.r, curData.g, curData.b));
            curHat.shelf = this;
            curHat.saveFileId = hatId + i;
            BuildFlair(curHat, curData);
        }
    }

    public static void BuildFlair(SaveHat curHat)
    {
        SaveHatListEntry curData = GetSaveFile(curHat.saveFileId);
        if(curData != null)
        {
            BuildFlair(curHat, curData);
        }
    }

    public static void BuildFlair(SaveHat curHat, SaveHatListEntry curData)
    {
        if (curHat.flairLoaded) return;
        curHat.flairLoaded = true;
        foreach (var entry in curData.flair)
        {
            HatFlair newFlair = (HatFlair)Instantiate(globalHatFlairPrefab, curHat.transform, false);
            newFlair.transform.localPosition = entry.position.V3;
            newFlair.transform.localRotation = entry.rotation.Q;
            newFlair.transform.SetParent(null);
            newFlair.GetComponent<Sticky>().AttachToBody(curHat.GetComponent<Rigidbody>());
            newFlair.flairIcon = entry.icon;
            newFlair.FlairId = entry.id;
            newFlair.flairSize = entry.size;
            Debug.Log("Restored flair " + newFlair.FlairId + " to hat " + curHat.saveFileId);
        }
    }

    internal void RemoveFlair(SaveHat saveHat, HatFlair flair)
    {
        SaveHatListEntry curData = null;
        if (!saveData.saveHats.ContainsKey(saveHat.saveFileId))
        {
            Debug.LogError("Invalid save file ID: " + saveHat.saveFileId, this);
            return;
        }
        curData = saveData.saveHats[saveHat.saveFileId];
        var removedCount = curData.flair.RemoveAll(new FlairIdSearch(flair.FlairId).Matches);
        Debug.Log("Removed flair " + flair.FlairId + " from hat " + saveHat.saveFileId+ "; removedCount=" + removedCount);
        Save();
    }

    internal void AddFlair(SaveHat saveHat, HatFlair flair)
    {
        SaveHatListEntry curData = null;
        if (!saveData.saveHats.ContainsKey(saveHat.saveFileId))
        {
            Debug.LogError("Invalid save file ID: " + saveHat.saveFileId, this);
            return;
        }
        curData = saveData.saveHats[saveHat.saveFileId];
        curData.flair.Add(new SaveHatFlairEntry(saveHat, flair));
        Debug.Log("Added flair " + flair.FlairId + " to hat " + saveHat.saveFileId);
        Save();
    }
}

internal class FlairIdSearch
{
    private string flairId;

    public FlairIdSearch(string flairId)
    {
        this.flairId = flairId;
    }

    public bool Matches(SaveHatFlairEntry entry)
    {
        return flairId.Equals(entry.id);
    }
}

[Serializable]
public class SaveHatData
{
    public Dictionary<int, SaveHatListEntry> saveHats = new Dictionary<int, SaveHatListEntry>();
}

[Serializable]
public class SaveHatListEntry
{
    public List<SaveHatFlairEntry> flair;
    public float b;
    public float g;
    public float r;
    public int coins;
}

[Serializable]
public class SaveHatFlairEntry
{
    public string id;
    public Vector3Serializer position;
    public QuaternionSerializer rotation;
    public string icon;
    public float size;

    public SaveHatFlairEntry() { }

    public SaveHatFlairEntry(SaveHat hat, HatFlair flair)
    {
        this.id = flair.FlairId;
        Transform originalParent = flair.transform.parent;
        flair.transform.parent = hat.transform;
        this.position = new Vector3Serializer().Fill(flair.transform.localPosition);
        this.rotation = new QuaternionSerializer().Fill(flair.transform.localRotation);
        flair.transform.parent = originalParent;
        this.icon = flair.flairIcon;
        this.size = flair.flairSize;
    }

    public override string ToString()
    {
        return "FlairId{icon=" + icon + ", size=" + size + "}";
    }
}