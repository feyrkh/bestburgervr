using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HappinessScoreDisplay : MonoBehaviour
{
    public float baseSecondsForBurger = 6f;
    public float secondsPerIngredient = 1.5f;
    float tipTotal;
    TipJar tipJar;

    public void Awake()
    {
        tipJar = GameObject.FindObjectOfType<TipJar>();
    }

    public virtual IEnumerator ScoreOrder(string[] desiredIngredients, CompletedBurger completedBurger, float timeSinceOrderStarted)
    {
        HideScores();
        float speed = CalcSpeedScore(desiredIngredients, timeSinceOrderStarted);
        float accuracy = CalcAccuracyScore(desiredIngredients, completedBurger.ingredients);
        float neatness = CalcNeatnessScore(completedBurger);
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

    private float CalcNeatnessScore(CompletedBurger completedBurger)
    {
        Debug.Log("prevAxisSlop=" + completedBurger.prevAxisSloppiness + ", baseAxisSlop=" + completedBurger.baseAxisSloppiness);
        float totalSloppiness = (completedBurger.prevAxisSloppiness + completedBurger.baseAxisSloppiness) / 2 * 1300;
        if (totalSloppiness > 100) totalSloppiness = 100;
        Debug.Log("NeatnessScore=" + (100f - totalSloppiness));
        return 100f - totalSloppiness;
    }

    private float CalcAccuracyScore(string[] desiredIngredients, string[] actualIngredients)
    {
        int missingErrors = 0;
        int extraErrors = 0;
        int outOfOrderErrors = 0;
        float missingPenalty = 150 / desiredIngredients.Length;
        float extraPenalty = missingPenalty / 5;
        float outOfOrderPenalty = 15 / desiredIngredients.Length;
        if (desiredIngredients.Length == 0) return 100;
        if (actualIngredients.Length == 0) return 0;

        Dictionary<String, int> excessIngredients = new Dictionary<string, int>();
        foreach (string desired in desiredIngredients)
        {
            int val = 0;
            excessIngredients.TryGetValue(desired, out val);
            excessIngredients[desired] = val + 1;
        }
        foreach (string actual in actualIngredients)
        {
            int val = 0;
            excessIngredients.TryGetValue(actual, out val);
            excessIngredients[actual] = val - 1;
        }
        foreach(int val in excessIngredients.Values)
        {
            if (val > 0) missingErrors += val;
            if (val < 0) extraErrors -= val;
        }
        outOfOrderErrors = GetOutOfOrderElements(desiredIngredients, actualIngredients);

        float accuracy = 100 - (extraErrors * extraPenalty) - (missingErrors * missingPenalty) - (outOfOrderErrors * outOfOrderPenalty);
        if (accuracy < 0) accuracy = 0;
        Debug.Log("AccuracyScore=" +accuracy+", missing="+missingErrors+", extra="+extraErrors+", outOfOrder="+outOfOrderErrors);
        return accuracy;
    }

    private int GetOutOfOrderElements(string[] source, string[] dest)
    {
        DiffEngine differ = new DiffEngine();
        differ.ProcessDiff(new DiffList_TextFile(source), new DiffList_TextFile(dest));
        ArrayList diffs = differ.DiffReport();
        diffs.TrimToSize();
        Debug.Log("Diffs found: " + diffs.Count);
        int errors = 0;
        foreach (DiffResultSpan diff in diffs)
        {
            if (!diff.Status.Equals("NoChange"))
            {
                errors += diff.Length;
            }
            Debug.Log("diff: " + diff);
        }
        return diffs.Count / 2;
    }

    private float CalcSpeedScore(string[] desiredIngredients, float timeSinceOrderStarted)
    {
        if (desiredIngredients.Length == 0) return 100;
        float expectedTime = baseSecondsForBurger + secondsPerIngredient * desiredIngredients.Length;
        float speedPenalty = 50 * (timeSinceOrderStarted / expectedTime) - 27;
        if (speedPenalty > 100) speedPenalty = 100;
        if (speedPenalty < 0) speedPenalty = 0;
        Debug.Log("SpeedScore=" + (100-speedPenalty) +", timesinceOrderStarted="+timeSinceOrderStarted+", expectedTime="+expectedTime);
        return 100 - speedPenalty;
    }

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
        if (score >= 90)
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
