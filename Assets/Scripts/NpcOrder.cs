using UnityEngine;
using System.Collections;

public class NpcOrder : MonoBehaviour {
    public Transform orderPosition;
    public IngredientsList orderLabelPrefab;
    public Transform trayPosition;
    IngredientsList orderLabel;
    bool acceptingOrders = true;
    string[] desiredIngredients;
    string[] actualIngredients;
    float timeSinceOrderStarted;
    CompletedBurger completedBurger;
    public HappinessScoreDisplay scorePrefab;
    public Transform scorePosition;

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
            acceptingOrders = false;
            Debug.Log("Received a food item, taking it", this);
            completedBurger = other.transform.parent.gameObject.GetComponent<CompletedBurger>();
            Debug.Log("Completed burger", completedBurger);
            actualIngredients = completedBurger.ingredients;
            var tray = other.transform.parent.gameObject;
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
            Destroy(orderLabel.gameObject);
            orderLabel = null;
            StartCoroutine("FinishOrder", tray);
        }
    }

    IEnumerator FinishOrder(GameObject tray)
    {
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
    }
}
