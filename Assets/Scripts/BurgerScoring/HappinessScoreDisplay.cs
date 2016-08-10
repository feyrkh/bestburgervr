using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HappinessScoreDisplay : MonoBehaviour
{
    public float baseSecondsForBurger = 5f;
    public float secondsPerIngredient = 2f;
    float tipTotal;
    TipJar tipJar;

    public void Awake()
    {
        tipJar = GameObject.FindObjectOfType<TipJar>();
    }

    public virtual IEnumerator ScoreOrder(string[] desiredIngredients, CompletedBurger completedBurger, float timeSinceOrderStarted)
    {
        HideScores();
        float speed = LevelManager.Instance.burgerScorer.CalcSpeedScore(desiredIngredients, timeSinceOrderStarted);
        float accuracy = LevelManager.Instance.burgerScorer.CalcAccuracyScore(desiredIngredients, completedBurger.ingredients);
        float neatness = LevelManager.Instance.burgerScorer.CalcNeatnessScore(completedBurger);
        tipTotal = 0;
        yield return new WaitForSeconds(0.5f);
        SetSpeedScore(speed);
        yield return new WaitForSeconds(0.5f);
        SetAccuracyScore(accuracy);
        yield return new WaitForSeconds(0.5f);
        SetNeatnessScore(neatness);
        if(tipJar != null)
            yield return tipJar.SpawnCoinsCoroutine(tipTotal);
        yield return new WaitForSeconds(4f);
    }
    private static UnityEngine.Random rand = new UnityEngine.Random();


    public void HideScores()
    {
        MeshRenderer score = transform.FindChild("neatnessScore").GetComponent<MeshRenderer>();
        score.gameObject.SetActive(false);
        score = transform.FindChild("speedScore").GetComponent<MeshRenderer>();
        score.gameObject.SetActive(false);
        score = transform.FindChild("accuracyScore").GetComponent<MeshRenderer>();
        score.gameObject.SetActive(false);
    }

    public void SetNeatnessScore(float neatness)
    {
        MeshRenderer neatnessScore = transform.FindChild("neatnessScore").GetComponent<MeshRenderer>();
        ApplyScoreTexture(neatness, neatnessScore);
        neatnessScore.gameObject.SetActive(true);
    }

    public void SetSpeedScore(float speed)
    {
        MeshRenderer speedScore = transform.FindChild("speedScore").GetComponent<MeshRenderer>();
        ApplyScoreTexture(speed, speedScore);
        speedScore.gameObject.SetActive(true);
    }

    public void SetAccuracyScore(float accuracy)
    {
        MeshRenderer accuracyScore = transform.FindChild("accuracyScore").GetComponent<MeshRenderer>();
        ApplyScoreTexture(accuracy, accuracyScore);
        accuracyScore.gameObject.SetActive(true);
    }

    public void SetScores(float neatness, float speed, float accuracy)
    {
        SetNeatnessScore(neatness);
        SetSpeedScore(speed);
        SetAccuracyScore(accuracy);
    }

    private void ApplyScoreTexture(float score, MeshRenderer label) {
        string texture = "neutral";
        float bonusScaling = LevelManager.Instance.settings.bonusScaling;
        if (score >= 88)
        {
            tipTotal += 0.3f * bonusScaling;
            texture = "very_happy";
        }
        else if (score >= 70)
        {
            tipTotal += .15f * bonusScaling;
            texture = "happy";
        }
        else if (score >= 50)
        {
            tipTotal += .05f * bonusScaling;
            texture = "neutral";
        }
        else if (score >= 25)
        {
            tipTotal -= 0.2f;
            texture = "unhappy";
        }
        else
        {
            tipTotal -= .75f;
            texture = "very_unhappy";
        }
        Texture textureResource = Resources.Load<Texture>(texture);
        label.material.mainTexture = textureResource;
    }
}
