using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public class SaveHatListEntry
{
    public List<SaveHatFlairEntry> flair;
    public float b;
    public float g;
    public float r;
    public float coins;
    public List<SaveHatFlairEntry> toothpickFlair;
}

[Serializable]
public class SaveHatFlairEntry
{
    public string id;
    public Vector3Serializer position;
    public QuaternionSerializer rotation;
    public string icon;
    public float size;
    public bool shiny;

    public SaveHatFlairEntry() { }

    public SaveHatFlairEntry(GameObject hat, HatFlair flair)
    {
        this.id = flair.FlairId;
        Transform originalParent = flair.transform.parent;
        flair.transform.parent = hat.transform;
        this.position = new Vector3Serializer().Fill(flair.transform.localPosition);
        this.rotation = new QuaternionSerializer().Fill(flair.transform.localRotation);
        flair.transform.parent = originalParent;
        this.icon = flair.flairIcon;
        this.size = flair.flairSize;
        this.shiny = flair.flairShiny == 1;
    }

    public override string ToString()
    {
        return "FlairId{icon=" + icon + ", size=" + size + ", shiny=" + shiny + "}";
    }
}

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
        BuildToothpickFlair();
    }
    
    public static SaveHatListEntry GetSaveFile(int id)
    {
        if(saveData.saveHats.ContainsKey(id))
            return saveData.saveHats[id];
        return null;
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
                Debug.LogError("Failed to load save file" + e.Message);
            }
        }
        if (saveData == null)
        {
            saveData = new SaveHatData();
        }
    }

    private void BuildToothpickFlair()
    {
        int saveFileId = LevelManager.Instance.currentlyLoadedSaveFile;
        Debug.Log("Restoring toothpick flair for save file " + saveFileId);
        if (!saveData.saveHats.ContainsKey(saveFileId))
        {
            Debug.Log("No save file for " + saveFileId + " found");
            return;
        }
        SaveHatListEntry curData = GetSaveFile(saveFileId);
        ToothpickJarFlair jar = GameObject.FindObjectOfType<ToothpickJarFlair>();
        if (jar == null)
        {
            Debug.Log("No toothpick jar found on the scene");
            return;
        }
        if(jar.flairLoaded)
        {
            Debug.Log("Flair already loaded on the toothpick jar");
            return;
        }
        jar.flairLoaded = true;
        if (curData != null && curData.toothpickFlair != null)
        {
            foreach (var entry in curData.toothpickFlair)
            {
                Debug.Log("Restoring toothpick flair " + entry.icon);
                HatFlair newFlair = ConstructFlair(jar.transform, entry);
                newFlair.GetComponent<Sticky>().AttachToBody(jar.GetComponent<Rigidbody>());
                Debug.Log("Restored flair " + newFlair.FlairId + " to jar " + saveFileId);
            }
        }
    }

    public static HatFlair ConstructFlair(Transform parent, SaveHatFlairEntry entry)
    {
        HatFlair newFlair = (HatFlair)Instantiate(globalHatFlairPrefab, parent, false);
        newFlair.transform.localPosition = entry.position.V3;
        newFlair.transform.localRotation = entry.rotation.Q;
        newFlair.transform.SetParent(null);
        newFlair.flairIcon = entry.icon;
        newFlair.FlairId = entry.id;
        newFlair.flairSize = entry.size;
        newFlair.flairShiny = entry.shiny ? 1 : 0;
        return newFlair;
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
                float hue = lowHueRange + (((highHueRange - lowHueRange) / hatCount) * i);
                Color color = Color.HSVToRGB(hue, 1f, 1f);
                curData.r = color.r;
                curData.g = color.g;
                curData.b = color.b;
                curData.flair = new List<SaveHatFlairEntry>();
                curData.coins = 2;
                saveData.saveHats[hatId + i] = curData;
                Debug.Log("Creating new hat. hue=" + hue + ", id=" + (hatId + i));
            }
            Vector3 position = new Vector3(0, yPos, spacePerHat / 2 + spacePerHat * i - (shelfWidth/2));
            SaveHat curHat = (SaveHat)Instantiate(hatPrefab, position + transform.position, Quaternion.Euler(0, -160, 0));
            Debug.Log("Instantiated hat", curHat);
            curHat.SetColor(new Color(curData.r, curData.g, curData.b));
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
            HatFlair newFlair = ConstructFlair(curHat.transform, entry);
            newFlair.GetComponent<Sticky>().AttachToBody(curHat.GetComponent<Rigidbody>());
            Debug.Log("Restored flair " + newFlair.FlairId + " to hat " + curHat.saveFileId);
        }
    }

    internal static void RemoveFlair(SaveHat saveHat, HatFlair flair)
    {
        SaveHatListEntry curData = null;
        if (!saveData.saveHats.ContainsKey(saveHat.saveFileId))
        {
            Debug.LogError("Invalid save file ID: " + saveHat.saveFileId, saveHat);
            return;
        }
        curData = saveData.saveHats[saveHat.saveFileId];
        var removedCount = curData.flair.RemoveAll(new FlairIdSearch(flair.FlairId).Matches);
        Debug.Log("Removed flair " + flair.FlairId + " from hat " + saveHat.saveFileId+ "; removedCount=" + removedCount);
        Save();
    }

    internal static void AddFlair(SaveHat saveHat, HatFlair flair)
    {
        SaveHatListEntry curData = null;
        if (!saveData.saveHats.ContainsKey(saveHat.saveFileId))
        {
            Debug.LogError("Invalid save file ID: " + saveHat.saveFileId, saveHat);
            return;
        }
        curData = saveData.saveHats[saveHat.saveFileId];
        curData.flair.Add(new SaveHatFlairEntry(saveHat.gameObject, flair));
        Debug.Log("Added flair " + flair.FlairId + " to hat " + saveHat.saveFileId);
        Save();
    }

    internal static void RemoveToothpickFlair(HatFlair flair)
    {
        SaveHatListEntry curData = null;
        int saveFileId = LevelManager.Instance.currentlyLoadedSaveFile;
        if (!saveData.saveHats.ContainsKey(saveFileId))
        {
            return;
        }
        curData = saveData.saveHats[saveFileId];
        var removedCount = curData.toothpickFlair.RemoveAll(new FlairIdSearch(flair.FlairId).Matches);
        Debug.Log("Removed flair " + flair.FlairId + " from toothpick jar " + saveFileId + "; removedCount=" + removedCount);
        Save();
    }

    internal static void AddToothpickFlair(GameObject toothpickJar, HatFlair flair)
    {
        SaveHatListEntry curData = null;
        int saveFileId = LevelManager.Instance.currentlyLoadedSaveFile;
        if (!saveData.saveHats.ContainsKey(saveFileId))
        {
            Debug.Log("No save file found: " + saveFileId);
            return;
        }
        curData = saveData.saveHats[saveFileId];
        if (curData.toothpickFlair == null) curData.toothpickFlair = new List<SaveHatFlairEntry>();
        curData.toothpickFlair.Add(new SaveHatFlairEntry(toothpickJar, flair));
        Debug.Log("Added flair " + flair.FlairId + " to toothpick jar " + saveFileId);
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
