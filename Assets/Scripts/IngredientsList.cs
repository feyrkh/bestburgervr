using UnityEngine;
using System.Collections;

public class IngredientsList : MonoBehaviour {
	public Transform ingredientListBottomPosition;
	public Transform toothpickSignPrefab;
	public MeshRenderer labelPrefab;

	void BuildIngredientLabel (string ingredient, Vector3 labelPosition, out MeshRenderer ingredientLabel)
	{
		ingredientLabel = (MeshRenderer)Instantiate (labelPrefab, labelPosition, Quaternion.identity);
		Texture texture = Resources.Load<Texture> (ingredient);
		ingredientLabel.material.mainTexture = texture;
		ingredientLabel.transform.SetParent (transform);
		ingredientLabel.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void SetIngredientList(string[] ingredients) {
		Vector3 labelPosition = ingredientListBottomPosition.localPosition;
		MeshRenderer ingredientLabel;
		foreach (string ingredient in ingredients) {
			BuildIngredientLabel (ingredient, labelPosition, out ingredientLabel);		
			ingredientLabel.transform.localPosition = labelPosition + new Vector3(0,0,-0.3f);
			BuildIngredientLabel (ingredient,  labelPosition, out ingredientLabel);		
			ingredientLabel.transform.localPosition = labelPosition + new Vector3(0,0,0.3f);
			ingredientLabel.transform.Rotate (new Vector3 (0, 180, 0));
			labelPosition = labelPosition + new Vector3 (0, 1.05f, 0);
		}
		toothpickSignPrefab.transform.localScale = new Vector3 (toothpickSignPrefab.transform.localScale.x, ingredients.Length + 2, toothpickSignPrefab.transform.localScale.z);
		toothpickSignPrefab.transform.localPosition = new Vector3 (0, (ingredients.Length + 2) / 2, 0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
