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

	void Awake () {
	    if(flairIcon == null || flairIcon == "")
        {
            flairIcon = validFlairs[Random.Range(0, validFlairs.Length)];
        }
        if(flairSize == 0)
        {
            flairSize = validSizes[Random.Range(0, validSizes.Length)];
        }
        transform.localScale = new Vector3(flairSize, 0.002f, flairSize);
        Texture textureResource = Resources.Load<Texture>(flairIcon);
        GetComponent<MeshRenderer>().material.mainTexture = textureResource;
    }
    
}
