using UnityEngine;
using System.Collections;
using VRTK;

public class VendingMachineItem : MonoBehaviour {
    public GameObject itemPrefab;
    public string itemName = "???";
    public string positionId = "A1";
    public int itemCost = 1;
    private GameObject currentItem;
    private bool vending = false;
    public VendingMachineLabel labelPrefab;
    
    public void Start()
    {
        InstantiateNewItem();
        InstantiateLabel();
    }

    private void InstantiateNewItem()
    {
        currentItem = (GameObject)Instantiate(itemPrefab, transform, false);
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
        float yOffset = -bounds.extents.y - 0.07f;
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
    private IEnumerator VendCoroutine()
    {
        yield return VendCoroutine(null);
    }

    private IEnumerator VendCoroutine(GameObject notifyOnVendComplete)
    {
        currentItem.transform.parent = null;
        yield return MoveUtil.MoveOverSeconds(currentItem, currentItem.transform.position + (Vector3.forward * 0.05f), 1);
        currentItem.GetComponent<Rigidbody>().detectCollisions = true;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.5f);
        InstantiateNewItem();
        vending = false;
        if(notifyOnVendComplete != null) {
            notifyOnVendComplete.SendMessage("OnVendComplete", this);
        }
    }
}
