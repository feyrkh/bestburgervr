using UnityEngine;
using System.Collections;

public class NpcOrder : MonoBehaviour {
    public Transform orderPosition;
    public IngredientsList orderLabelPrefab;
    IngredientsList orderLabel; 

    public void SetOrder(string[] ingredients)
    {
        if (orderLabel != null)
        {
            Destroy(orderLabel.gameObject);
            orderLabel = null;
        }
        if (ingredients == null) return;
        orderLabel = (IngredientsList) Instantiate(orderLabelPrefab, orderPosition.position, orderPosition.rotation);
        orderLabel.SetIngredientList(ingredients);
        orderLabel.transform.SetParent(gameObject.transform);
    }

    public string[] GetOrder()
    {
        if (orderLabel == null) return null;
        return orderLabel.GetIngredientList();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with an NPC", other);
        if(other.transform.parent.gameObject.CompareTag("Food"))
        {
            Debug.Log("Received a food item, just consuming it", this);
            Destroy(other.transform.parent.gameObject);
            SetOrder(null);
        }
    }
}
