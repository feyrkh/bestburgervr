using UnityEngine;
using System.Collections;
using System;

public class IngredientsList : MonoBehaviour {
	public Transform ingredientListBottomPosition = null;
	public Transform signPrefab = null;
	public MeshRenderer labelPrefab = null;
	public string iconPrefix = "";
    public string[] ingredients = null;
    public float expectedHeight = 1f;
    public Transform sign = null;

	public bool testIngredientList = false;

	void BuildIngredientLabel (string ingredient, Vector3 labelPosition, out MeshRenderer ingredientLabel)
	{
		ingredientLabel = (MeshRenderer)Instantiate (labelPrefab, labelPosition, Quaternion.identity);
		Texture texture = Resources.Load<Texture> (iconPrefix+ingredient);
		ingredientLabel.material.mainTexture = texture;
		ingredientLabel.transform.SetParent (transform);
        if (texture == null) Debug.LogError("Invalid texture name: " + iconPrefix + ingredient, gameObject);
		float w = texture.width;
		float h = texture.height;
		float aspect = w / (float)h;
		ingredientLabel.transform.localScale = new Vector3 (aspect * expectedHeight, expectedHeight, 1);
	}

    public string[] GetIngredientList()
    {
        return ingredients;
    }

    public void SetIngredientList(string[] ingredients) {
		Vector3 labelPosition = ingredientListBottomPosition.localPosition;
		MeshRenderer ingredientLabel;
        this.ingredients = ingredients;
		int i = 0;
		int scale = ingredients.Length + 2;
		foreach (string ingredient in ingredients) {
            /*
            if (i == 0 && ingredient == "bottom_bun") {
				scale--;
				continue;
			}
            */
			i++;
            /*
			if (i == ingredients.Length - 1 && ingredient == "top_bun") {
				scale--;
				continue;
			}
            */
			//BuildIngredientLabel (ingredient, labelPosition, out ingredientLabel);		
			//ingredientLabel.transform.localPosition = labelPosition + new Vector3(0,0,-0.01f);
			BuildIngredientLabel (ingredient,  labelPosition, out ingredientLabel);		
			ingredientLabel.transform.localPosition = labelPosition + new Vector3(0,0, 0.01f);
			ingredientLabel.transform.Rotate (new Vector3 (0, 180, 0));
			labelPosition = labelPosition + new Vector3 (0, ingredientLabel.transform.localScale.y, 0);
		}
		signPrefab.transform.localScale = new Vector3 (signPrefab.transform.localScale.x, scale * expectedHeight, signPrefab.transform.localScale.z);
		signPrefab.transform.localPosition = new Vector3 (0,labelPosition.y - (scale*expectedHeight / 2), 0);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("RunTest");
	}

	IEnumerator RunTest() {
		while(true) {
			if(testIngredientList == true) {
				SetIngredientList(new string[] {"bottom_bun", "meat", "cheese", "tomato", "top_bun"});
				testIngredientList = false;
			}
			yield return new WaitForSeconds (2);
		}
	}
	
}
