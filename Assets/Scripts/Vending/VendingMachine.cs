using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class VendingMachine : MonoBehaviour {
    private int coinsInserted = 0;
    public VRTK_ObjectTooltip coinsInsertedLabel;
    public Transform coinReturnPoint;
    public Coin oneDollarCoinPrefab;
    public Coin fiveDollarCoinPrefab;
    public Coin twentyDollarCoinPrefab;
    public float timeBetweenRefundCoins = 0.08f;
    private bool refunding = false;

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
            coinsInsertedLabel.displayText = "Credit: $" + coinsInserted;
            coinsInsertedLabel.Reset();
        }
    }

    internal void CoinInserted(Coin coin)
    {
        coinsInserted += coin.coinValue;
        UpdateLabels();
        Destroy(coin.gameObject);
    }

    public void OnButtonPushed(string buttonName)
    {
        Debug.Log("Vending machine button pressed: " + buttonName);
        if(buttonName.Equals("refund"))
        {
            StartCoroutine(RefundCoins());
        }
    }

    internal IEnumerator RefundCoins()
    {
        if (!refunding) {
            Debug.Log("Starting refund process for $"+coinsInserted);
            refunding = true;
            if (twentyDollarCoinPrefab != null)
            {
                while (coinsInserted > 20)
                {
                    yield return RefundCoin(twentyDollarCoinPrefab, 20);
                }
            }
            if (fiveDollarCoinPrefab != null)
            {
                while (coinsInserted > 5)
                {
                    yield return RefundCoin(fiveDollarCoinPrefab, 5);
                }
            }
            if (oneDollarCoinPrefab != null)
            {
                while (coinsInserted > 0)
                {
                    yield return RefundCoin(oneDollarCoinPrefab, 1);
                }
            }
            refunding = false;
        } else
        {
            Debug.Log("Already refunding");
        }
    }

    private IEnumerator RefundCoin(Coin coinPrefab, int coinValue)
    {
        Coin newCoin = Instantiate(coinPrefab);
        newCoin.transform.position = coinReturnPoint.position;
        newCoin.coinValue = coinValue;
        coinsInserted -= coinValue;
        UpdateLabels();
        Debug.Log("Refunded " + coinValue);
        yield return new WaitForSeconds(timeBetweenRefundCoins);
    }
}
