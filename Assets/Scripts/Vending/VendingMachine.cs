using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class VendingMachine : MonoBehaviour {
    private double coinsInserted = 0;
    public VRTK_ObjectTooltip coinsInsertedLabel;
    public VRTK_ObjectTooltip selectionLabel;
    public Transform coinReturnPoint;
    public Coin quarterDollarCoinPrefab;
    public Coin oneDollarCoinPrefab;
    public Coin fiveDollarCoinPrefab;
    public Coin twentyDollarCoinPrefab;
    public float timeBetweenRefundCoins = 0.08f;
    private bool refunding = false;
    private bool currentlyVending = false;
    private string selectedLetter = "?";
    private string selectedNumber = "?";

    public void Awake()
    {
        if(UnityEngine.Random.Range(0, 100) > 98)
        {
            coinsInserted = 1;
        }
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        if (coinsInserted <= 0)
        {
            coinsInserted = 0;
            coinsInsertedLabel.displayText = "Insert Coin";
            coinsInsertedLabel.Reset();
        }
        else
        {
            coinsInsertedLabel.displayText = "Credit: $" + String.Format("{0:N2}", coinsInserted);
            coinsInsertedLabel.Reset();
        }
        string selectionText;
        if(currentlyVending)
        {
            selectionText = "Vending: "+selectedLetter+selectedNumber;
        } else if(refunding)
        {
            selectionText = "Take Change";
        } else
        {
            selectionText = "Selection: " + selectedLetter + selectedNumber;
        }
        selectionLabel.displayText = selectionText;
        selectionLabel.Reset();
    }

    internal void CoinInserted(Coin coin)
    {
        coinsInserted += coin.coinValue;
        UpdateLabels();
        Destroy(coin.gameObject);
    }

    public void OnButtonPushed(string buttonName)
    {
        if(currentlyVending || refunding)
        {
            return;
        }
        Debug.Log("Vending machine button pressed: " + buttonName);
        if(buttonName.Equals("refund"))
        {
            StartCoroutine(RefundCoins());
        }
        if(buttonName.Length == 1)
        {
            if (buttonName[0] >= 'A' && buttonName[0] <= 'F')
            {
                selectedLetter = buttonName;
            }
            if (buttonName[0] >= '1' && buttonName[0] <= '9')
            {
                selectedNumber = buttonName;
            }
            UpdateLabels();
        }
        if(selectedLetter != "?" && selectedNumber != "?")
        {
            Transform selected = transform.FindChild("items/" + selectedLetter + "/" + selectedNumber);
            VendingMachineItem selectedItem = null;
            GameObject vendingItem = null;
            if (selected != null) selectedItem = selected.GetComponent<VendingMachineItem>();
            if(selectedItem == null) 
            {
                selectionLabel.displayText = "No Selection: " + selectedLetter + selectedNumber;
                selectionLabel.Reset();
                ResetSelection();
                return;
            }
            if(selectedItem.itemCost > coinsInserted)
            {
                selectionLabel.displayText = "Insert $"+(selectedItem.itemCost-coinsInserted)+" for " + selectedLetter + selectedNumber;
                selectionLabel.Reset();
                ResetSelection();
                return;
            }
            currentlyVending = true;
            coinsInserted -= selectedItem.itemCost;
            UpdateLabels();
            selectedItem.Vend(this.gameObject);
        }
    }

    public void OnVendComplete()
    {
        Debug.Log("Vending complete");
        LevelManager.Instance.CountCoins();
        currentlyVending = false;
        ResetSelection();
        UpdateLabels();
    }

    private void ResetSelection()
    {
        selectedLetter = "?";
        selectedNumber = "?";
    }

    internal IEnumerator RefundCoins()
    {
        if (!refunding && !currentlyVending) {
            Debug.Log("Starting refund process for $"+coinsInserted);
            refunding = true;
            if (twentyDollarCoinPrefab != null)
            {
                while (coinsInserted >= 20)
                {
                    yield return RefundCoin(twentyDollarCoinPrefab, 20);
                }
            }
            if (fiveDollarCoinPrefab != null)
            {
                while (coinsInserted >= 5)
                {
                    yield return RefundCoin(fiveDollarCoinPrefab, 5);
                }
            }
            if (oneDollarCoinPrefab != null)
            {
                while (coinsInserted >= 1)
                {
                    yield return RefundCoin(oneDollarCoinPrefab, 1);
                }
            }
            if (quarterDollarCoinPrefab != null)
            {
                while (coinsInserted > 0)
                {
                    yield return RefundCoin(quarterDollarCoinPrefab, 0.25f);
                }
            }
            refunding = false;
        } else
        {
            Debug.Log("Already refunding");
        }
    }

    private IEnumerator RefundCoin(Coin coinPrefab, float coinValue)
    {
        Coin newCoin = Instantiate(coinPrefab);
        newCoin.transform.position = coinReturnPoint.position;
        newCoin.coinValue = coinValue;
        coinsInserted -= coinValue;
        UpdateLabels();
        Debug.Log("Refunded " + coinValue);
        yield return new WaitForSeconds(timeBetweenRefundCoins);
        LevelManager.Instance.CountCoins();
    }
}
