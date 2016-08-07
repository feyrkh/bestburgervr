using UnityEngine;
using System.Collections;

public class HatFlair : MonoBehaviour {
    public static float[] validSizes = { 0.08f, 0.06f, 0.06f, 0.06f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f, 0.04f};

    public static string[] validFlairs =
    {
        "accuracy",
        "bottom_bun",
        "cheese",
        "happy",
        "ketchup",
        "lettuce",
        "meat",
        "mustard",
        "neatness",
        "neutral",
        "speed",
        "tomato",
        "top_bun",
        "unhappy",
        "very_happy",
        "very_unhappy"
    };

    public string flairIcon = null;
    public float flairSize = 0;
    public int flairShiny = -1;
    private string flairId = null;

    public string FlairId
    {
        get
        {
            return flairId;
        }

        set
        {
            flairId = value;
        }
    }

    void Start () {
        StartCoroutine("UpdateFlairSettings");
    }
    
    IEnumerator UpdateFlairSettings()
    {
        yield return null;
        if(flairShiny < 0)
        {
            flairShiny = Random.Range(0, 100) > 98 ? 1 : 0;
        }
        if (FlairId == null)
        {
            FlairId = System.Guid.NewGuid().ToString();
            Debug.Log("new flair id generated: " + FlairId);
        }
        if (flairIcon == null || flairIcon == "")
        {
            flairIcon = ChooseRandomFlairIcon();
        }
        if (flairSize == 0)
        {
            flairSize = validSizes[Random.Range(0, validSizes.Length)];
        }
        ApplyFlairSettings();
    }

    public void OnPreVend(string itemName)
    {
        if (itemName != null && itemName != "")
        {
            flairIcon = itemName;
            ApplyFlairSettings();
        }
    }

    public void ApplyFlairSettings()
    {
        transform.localScale = new Vector3(flairSize, 0.002f, flairSize);
        Texture textureResource = Resources.Load<Texture>(flairIcon);
        GetComponent<MeshRenderer>().material.mainTexture = textureResource;
        GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_Metallic"), flairShiny > 0 ? .95f : 0.05f);
    }

    public static string ChooseRandomFlairIcon()
    {
        return validFlairs[Random.Range(0, validFlairs.Length)];
    }
}
