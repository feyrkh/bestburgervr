using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BurgerToothpick : MonoBehaviour {
    private CompletedBurger stuckIn;
    public int ingredientLayer = 10;
    public float toothpickLength = 0.1f;
    private int layer = -1;
    public CompletedBurger completedBurgerPrefab;
    

    public void Start()
    {
        layer = 1 << ingredientLayer;
        toothpickLength = transform.FindChild("pick").GetComponent<Renderer>().bounds.extents.y * 2;
    }


    public void OnBeginInteraction(NewtonVR.NVRHand hand)
    {
        Debug.Log("Picked up a toothpick");
        var colliders = transform.GetComponentsInChildren<Collider>();
        SetCollidersEnabled(false);

    }

    private void SetCollidersEnabled(bool v)
    {
        foreach(var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = v;
        }
    }

    public void OnEndInteraction()
    {
        Debug.Log("Dropping a toothpick, checking to see if there's a nearby ingredient");
        RaycastHit[] ingredientHits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), toothpickLength, layer);
        if(ingredientHits.Length == 0)
        {
            // No ingredients intersected this toothpick
            SetCollidersEnabled(true);
            return;
        }
        int count = 0;
        do
        {
            count = ingredientHits.Length;
            toothpickLength += 0.03f;
            ingredientHits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), toothpickLength, layer);
        } while (ingredientHits.Length != count);
        // Sort the ingredients according to their distance along the toothpick
        Array.Sort(ingredientHits, new RaycastHitComparer());
        GameObject[] ingredients = new GameObject[ingredientHits.Length];
        for (int i = 0; i < ingredientHits.Length; i++)
        {
            ingredients[i] = ingredientHits[i].collider.gameObject;
        }
        LockBurger(ingredients);
    }

    private void LockBurger(GameObject[] ingredients)
    {
        Debug.Log("Finalizing burger");
        Vector3 baseCenterAxis = Vector3.zero;
        Vector3 previousCenterAxis = Vector3.zero;
        float totalDistanceFromBaseCenterAxis = 0;
        float totalDistanceFromPreviousCenterAxis = 0;
        NewtonVR.NVRInteractables.Deregister(GetComponent<NewtonVR.NVRInteractable>());
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(GetComponent<Rigidbody>());

        List<string> ingredientNames = new List<string>();
        foreach (GameObject i in ingredients)
        {
            if (i.GetComponent<Ingredient>() == null) continue;
            Debug.Log(i.GetComponent<Ingredient>().ingredientName);
            ingredientNames.Add(i.GetComponent<Ingredient>().ingredientName);
            NewtonVR.NVRInteractables.Deregister(i.GetComponent<NewtonVR.NVRInteractable>());
            Destroy(i.GetComponent<Ingredient>());
            Destroy(i.GetComponent<Rigidbody>());
            Destroy(i.GetComponent<NewtonVR.NVRInteractableItem>());
            if (baseCenterAxis == Vector3.zero)
            {
                baseCenterAxis = i.transform.position;
                previousCenterAxis = baseCenterAxis;
            }
            else
            {
                Vector3 curDistFromBaseAxis = (baseCenterAxis - i.transform.position);
                curDistFromBaseAxis.Set(curDistFromBaseAxis.x, 0, curDistFromBaseAxis.z);
                totalDistanceFromBaseCenterAxis += curDistFromBaseAxis.magnitude;
                Vector3 curDistFromPrevAxis = (previousCenterAxis - i.transform.position);
                curDistFromPrevAxis.Set(curDistFromPrevAxis.x, 0, curDistFromPrevAxis.z);
                totalDistanceFromPreviousCenterAxis += curDistFromPrevAxis.magnitude;

                previousCenterAxis = i.transform.position;
            }
        }
        CompletedBurger completedBurger = (CompletedBurger)Instantiate(completedBurgerPrefab, transform.position, Quaternion.identity);
        completedBurger.ingredients = ingredientNames.ToArray();
        this.transform.SetParent(completedBurger.transform);
        int divideBy = Mathf.Max(ingredients.Length - 1, 1);

        completedBurger.baseAxisSloppiness = totalDistanceFromBaseCenterAxis / divideBy;
        completedBurger.prevAxisSloppiness = totalDistanceFromPreviousCenterAxis / divideBy;
        if (ingredients.Length <= 2)
        {
            completedBurger.baseAxisSloppiness = 0.3f;
            completedBurger.prevAxisSloppiness = 0.3f;
        }
        foreach (GameObject i in ingredients)
        {
            i.transform.SetParent(completedBurger.transform);
        }
        completedBurger.gameObject.SetActive(false);
        completedBurger.gameObject.SetActive(true);

        completedBurger.gameObject.AddComponent<NewtonVR.NVRInteractableItem>();
        //Destroy(burgerDetectionArea.gameObject);
    }
}

internal class RaycastHitComparer : IComparer<RaycastHit>
{
    public int Compare(RaycastHit x, RaycastHit y)
    {
        if (x.distance < y.distance) return 1;
        if (y.distance < x.distance) return -1;
        return 0;
    }
}