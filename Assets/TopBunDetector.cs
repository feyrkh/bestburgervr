using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopBunDetector : MonoBehaviour {

    public Vector3 startPoint;
	public Vector3 movementPerSecond = new Vector3(0, 0.6f, 0);
    public Color detectedColor;
    public Color notDetectedColor;
    public int placardsInArea = 0;
    public float timeUntilOrderComplete = 1f;
    public float placardResidentSeconds = 0.1f;
	private static float placardDetectionInterval = 0.1f;
	public CompletedBurger completedBurgerPrefab;
    public List<GameObject> orderPlacards = new List<GameObject>();
    public Transform burgerDetectionArea;

	// Use this for initialization
	void Start () {
		StartCoroutine ("CheckForPlacard");
        if (burgerDetectionArea == null) burgerDetectionArea = transform.parent.FindChild("burgerDetectionArea");
        startPoint = burgerDetectionArea.transform.localPosition;
    }

	IEnumerator ScanUpward()
    {
        burgerDetectionArea.transform.localPosition = startPoint;
        List<GameObject> ingredients = new List<GameObject> ();
		float secondsSinceLastIngredient = 0;
		bool foundNewIngredients = false;
        GameObject firstPlacard = orderPlacards[0];
        var deltaTime = 0.008f;
        do {
			foundNewIngredients = false;
            Bounds bounds = burgerDetectionArea.GetComponent<Renderer>().bounds;
			Collider[] foundItems = Physics.OverlapBox(bounds.center, bounds.extents, burgerDetectionArea.transform.rotation);
			foreach(Collider foundItem in foundItems) {
				Ingredient foundIngredient = foundItem.GetComponent<Ingredient>();
				if(foundIngredient && !ingredients.Contains(foundIngredient.gameObject)) {

                    NewtonVR.NVRInteractableItem nvri = foundIngredient.GetComponent<NewtonVR.NVRInteractableItem>();
                    if (nvri.AttachedHand == null)
                    {
                        foundNewIngredients = true;
                        ingredients.Add(foundIngredient.gameObject);
                        Debug.Log("Found ingredient: " + foundIngredient.ingredientName, foundIngredient.gameObject);
                    }
				}
			}
			if(foundNewIngredients) {
				secondsSinceLastIngredient = 0;
			}  else {
				secondsSinceLastIngredient += deltaTime;
			}
            burgerDetectionArea.transform.localPosition = burgerDetectionArea.transform.localPosition + movementPerSecond * deltaTime;
		} while(secondsSinceLastIngredient < 0.05f);
		transform.localPosition = startPoint;
        ingredients.TrimExcess();
        if (ingredients.Capacity != 0)
        {
            FinalizeBurger(transform.parent.gameObject, ingredients);
        }
        yield return null;
	}

	void FinalizeBurger(GameObject burgerScanner, List<GameObject> ingredients) {
        Debug.Log("Finalizing burger");
        GameObject previousIngredient = ingredients[0];
		Vector3 baseCenterAxis = Vector3.zero;
		Vector3 previousCenterAxis =  Vector3.zero;
		float totalDistanceFromBaseCenterAxis = 0;
		float totalDistanceFromPreviousCenterAxis = 0;


        Destroy(burgerScanner.GetComponent<Ingredient>());
        Destroy(burgerScanner.GetComponent<Rigidbody>());
        string[] ingredientNames = new string[ingredients.Capacity];
		int idx = 0;
		foreach (GameObject i in ingredients) {
			Debug.Log (i.GetComponent<Ingredient>().ingredientName);
			ingredientNames[idx++] = i.GetComponent<Ingredient>().ingredientName;
            NewtonVR.NVRInteractables.Deregister(i.GetComponent<NewtonVR.NVRInteractable>());
			Destroy (i.GetComponent<Ingredient> ());
            Destroy(i.GetComponent<Rigidbody>());
           // Destroy(i.GetComponent<NewtonVR.NVRInteractableItem>());
            if (baseCenterAxis == Vector3.zero) {
				baseCenterAxis = i.transform.position;
				previousCenterAxis = baseCenterAxis;
			} else {
				Vector3 curDistFromBaseAxis = (baseCenterAxis - i.transform.position);
				curDistFromBaseAxis.Set (curDistFromBaseAxis.x, 0, curDistFromBaseAxis.z);
				totalDistanceFromBaseCenterAxis += curDistFromBaseAxis.magnitude;
				Vector3 curDistFromPrevAxis = (previousCenterAxis - i.transform.position);
				curDistFromPrevAxis.Set (curDistFromPrevAxis.x, 0, curDistFromPrevAxis.z);
				totalDistanceFromPreviousCenterAxis += curDistFromPrevAxis.magnitude;

				previousCenterAxis = i.transform.position;
			}
			previousIngredient = i;
		}
		CompletedBurger completedBurger = (CompletedBurger)Instantiate (completedBurgerPrefab, transform.position, Quaternion.identity);
		completedBurger.ingredients = ingredientNames;
        burgerScanner.transform.SetParent(completedBurger.transform);
        int divideBy = Mathf.Max(ingredients.Capacity - 1, 1);

		completedBurger.baseAxisSloppiness = totalDistanceFromBaseCenterAxis/divideBy;
		completedBurger.prevAxisSloppiness = totalDistanceFromPreviousCenterAxis/divideBy;
		foreach (GameObject i in ingredients) {
			i.transform.SetParent (completedBurger.transform);
        }
        completedBurger.gameObject.SetActive(false);
        completedBurger.gameObject.SetActive(true);
        
        completedBurger.gameObject.AddComponent<NewtonVR.NVRInteractableItem>();
        //Destroy(burgerDetectionArea.gameObject);
        burgerDetectionArea.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        this.gameObject.transform.parent.GetComponent<NewtonVR.NVRInteractable>().RefreshColliders();
	}
    

IEnumerator CheckForPlacard()
    {
		while (true) {
			if (placardsInArea > 0) {
                bool unattachedBunFound = false;
                for(int i=0;i<orderPlacards.Count;i++)
                {
                    NewtonVR.NVRInteractableItem nvri = orderPlacards[i].GetComponent<NewtonVR.NVRInteractableItem>();
                    if(nvri != null && nvri.AttachedHand == null)
                    {
                        unattachedBunFound = true;
                        break;
                    }
                }
                if (!unattachedBunFound)
                {
                    yield return new WaitForSeconds(placardDetectionInterval);
                    continue;
                }
                placardResidentSeconds += placardDetectionInterval;
				if (placardResidentSeconds >= timeUntilOrderComplete) {
						yield return ScanUpward ();
						placardResidentSeconds = 0;
				}
			} else if (placardResidentSeconds > 0) {
				placardResidentSeconds = 0;
			}
			yield return new WaitForSeconds(placardDetectionInterval);
		}
    }
	
    void OnTriggerEnter(Collider other)
    {
        Ingredient possibleTopBun = other.GetComponent<Ingredient>();

        if(possibleTopBun != null && possibleTopBun.ingredientName == "top_bun")
        {
            if(placardsInArea == 0)
            {
                this.GetComponent<MeshRenderer>().material.color = detectedColor;
            }
            orderPlacards.Add(other.gameObject);
            placardsInArea++;
        }
    }

    void OnTriggerExit(Collider other)
    {

        Ingredient possibleTopBun = other.GetComponent<Ingredient>();

        if (possibleTopBun != null && possibleTopBun.ingredientName == "top_bun")
        {
            placardsInArea--;
            orderPlacards.Remove(other.gameObject);
            if (placardsInArea == 0)
            {
                this.GetComponent<MeshRenderer>().material.color = notDetectedColor;
            }
        }
    }
}
