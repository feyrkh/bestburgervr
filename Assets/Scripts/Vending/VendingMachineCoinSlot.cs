using UnityEngine;
using System.Collections;

public class VendingMachineCoinSlot : MonoBehaviour {
    public Transform coinReturn;
    public VendingMachine vendingMachine;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered a coin slot", other);
        Coin coin = other.GetComponent<Coin>();
        if(coin == null)
        {
            // interactable items aside from coins get spit back out into the coin return
            // We have to check for NVRInteractableItem because the player's controllers & head can also trigger this, 
            // which causes bad things to happen
            if(other.GetComponent<NewtonVR.NVRInteractableItem>() != null) other.transform.position = coinReturn.position;
        } else 
        {
            Debug.Log("Coin inserted into a coin slot", coin);
            vendingMachine.CoinInserted(coin);
        }
    }
}
