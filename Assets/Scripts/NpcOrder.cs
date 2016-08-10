using UnityEngine;
using System.Collections;
using System;

public class NpcOrder : MonoBehaviour {
    public Transform orderPosition;
    public IngredientsList orderLabelPrefab;
    public Transform trayPosition;
    IngredientsList orderLabel;
    public bool acceptingOrders = true;
    string[] desiredIngredients;
    string[] actualIngredients;
    float timeSinceOrderStarted;
    CompletedBurger completedBurger;
    public HappinessScoreDisplay scorePrefab;
    public Transform scorePosition;

    public string[] GetDesiredIngredients()
    {
        return desiredIngredients;
    }

    public float GetTimeSinceOrderStarted()
    {
        return timeSinceOrderStarted;
    }

    public void Update()
    {
        if(orderLabel != null) 
            timeSinceOrderStarted += Time.deltaTime;
    }

    public void SetOrder(string[] ingredients)
    {
        timeSinceOrderStarted = 0;
        if (orderLabel != null)
        {
            Destroy(orderLabel.gameObject);
            orderLabel = null;
        }
        if (ingredients == null) return;
        orderLabel = (IngredientsList) Instantiate(orderLabelPrefab, orderPosition.position, orderPosition.rotation);
        orderLabel.SetIngredientList(ingredients);
        orderLabel.transform.SetParent(gameObject.transform);
        desiredIngredients = ingredients;
    }

    public string[] GetOrder()
    {
        if (orderLabel == null) return null;
        return orderLabel.GetIngredientList();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!acceptingOrders) return;
        if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Food"))
        {
            other.transform.parent.gameObject.tag = "Untagged";
            acceptingOrders = false;
            Debug.Log("Received a food item, taking it", this);
            completedBurger = other.transform.parent.gameObject.GetComponent<CompletedBurger>();
            ReceiveCompletedBurger(completedBurger);
        }
    }

    public void ReceiveCompletedBurger(CompletedBurger completedBurger)
    {
        Debug.Log("Completed burger", completedBurger);
        this.completedBurger = completedBurger;
        actualIngredients = completedBurger.ingredients;
        var tray = completedBurger.gameObject;
        completedBurger.container = tray;
        tray.transform.SetParent(this.transform);
        foreach (var i in tray.GetComponentsInChildren<NewtonVR.NVRInteractableItem>())
        {
            Destroy(i);
        }

        foreach (var i in tray.GetComponentsInChildren<Rigidbody>())
        {
            i.isKinematic = true;
        }

        tray.GetComponent<Rigidbody>().isKinematic = true;
        var trayInteractable = tray.GetComponent<NewtonVR.NVRInteractableItem>();
        if (trayInteractable != null)
        {
            Destroy(trayInteractable);
        }
        tray.GetComponent<Rigidbody>().isKinematic = true;
        if(orderLabel != null) Destroy(orderLabel.gameObject);
        orderLabel = null;
        StartCoroutine("FinishOrder", tray);
    }

    IEnumerator FinishOrder(GameObject tray)
    {
        /*
        yield return MoveUtil.MoveOverSeconds(tray, trayPosition.localPosition, 1f);
        HappinessScoreDisplay scorer = Instantiate<HappinessScoreDisplay>(scorePrefab);
        scorer.transform.SetParent(transform);
        scorer.transform.localPosition = scorePosition.localPosition;
        scorer.transform.localEulerAngles = scorePosition.localEulerAngles;
        if(scorer != null)
        {
            yield return scorer.ScoreOrder(desiredIngredients, completedBurger, timeSinceOrderStarted);
        }
        Destroy(scorer);
        SendMessage("OnTimeToLeave");
        */
        yield return LevelManager.Instance.waveRules.NpcServed(this, completedBurger);
    }
}
