using UnityEngine;
using System.Collections;
using VRTK;

public class VendingMachineItem : MonoBehaviour {
    public GameObject itemPrefab;
    public string itemName = "???";
    internal string positionId;
    public int itemCost = 1;
    public GameObject currentItem;
    private bool vending = false;
    public VendingMachineLabel labelPrefab;
    public float labelYOffset = -0.06f;
    
    public void Start()
    {
        positionId = this.transform.parent.name + this.name;
        InstantiateNewItem();
        InstantiateLabel();
    }

    private void InstantiateNewItem()
    {
        currentItem = (GameObject)Instantiate(itemPrefab, transform, false);
        currentItem.SendMessage("OnPreVend", itemName, SendMessageOptions.DontRequireReceiver);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.parent = null;
        currentItem.GetComponent<Rigidbody>().detectCollisions = false;
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void InstantiateLabel()
    {
        VendingMachineLabel label = (VendingMachineLabel)Instantiate(labelPrefab);
        Bounds bounds = VRTK.Utilities.getBounds(currentItem.transform);
        Bounds labelBounds = VRTK.Utilities.getBounds(labelPrefab.transform);
        //float yOffset = -bounds.extents.y - 0.01f - labelBounds.extents.y;
        //float yOffset = -bounds.extents.y - 0.07f;
        float yOffset = labelYOffset;
        Debug.Log("Setting y offset of " + yOffset);
        label.transform.position = transform.position + new Vector3(0, yOffset, 0);
        label.GetComponentInChildren<VRTK_ObjectTooltip>().displayText = positionId + ": $" + itemCost;
    }

    public bool Vend()
    {
        return Vend(null);
    }

    public bool Vend(GameObject notifyOnVendComplete)
    {
        if (vending) return false;
        vending = true;
        StartCoroutine("VendCoroutine", notifyOnVendComplete);
        return true;
    }

    private IEnumerator VendCoroutine(GameObject notifyOnVendComplete)
    {
        currentItem.transform.parent = null;
        yield return MoveUtil.MoveOverSeconds(currentItem, currentItem.transform.position - (Vector3.forward * 0.1f), 3);
        currentItem.GetComponent<Rigidbody>().detectCollisions = true;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.1f);
        currentItem.SendMessage("OnVendComplete", SendMessageOptions.DontRequireReceiver);
        yield return new WaitForSeconds(0.4f);
        InstantiateNewItem();
        vending = false;
        Debug.Log("OnVendComplete being called for " + notifyOnVendComplete, notifyOnVendComplete);
        if(notifyOnVendComplete != null) {
            notifyOnVendComplete.SendMessage("OnVendComplete", this);
        }
    }
}
