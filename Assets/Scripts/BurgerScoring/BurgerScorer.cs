using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BurgerScorer : MonoBehaviour
{
    public float baseSecondsForBurger = 5f;
    public float secondsPerIngredient = 2f;

    public virtual float CalcNeatnessScore(CompletedBurger completedBurger)
    {
        Debug.Log("prevAxisSlop=" + completedBurger.prevAxisSloppiness + ", baseAxisSlop=" + completedBurger.baseAxisSloppiness);
        float totalSloppiness = (completedBurger.prevAxisSloppiness + completedBurger.baseAxisSloppiness) / 2 * 1300;
        if (totalSloppiness > 100) totalSloppiness = 100;
        Debug.Log("NeatnessScore=" + (100f - totalSloppiness));
        return 100f - totalSloppiness;
    }

    public virtual float CalcAccuracyScore(string[] desiredIngredients, string[] actualIngredients)
    {
        int missingErrors = 0;
        int extraErrors = 0;
        int outOfOrderErrors = 0;
        float missingPenalty = 150 / desiredIngredients.Length;
        float extraPenalty = missingPenalty / 5;
        float outOfOrderPenalty = 15 / desiredIngredients.Length;
        if (desiredIngredients.Length == 0) return 100;
        if (actualIngredients.Length == 0) return 0;

        Dictionary<string, int> excessIngredients = new Dictionary<string, int>();
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
        foreach (int val in excessIngredients.Values)
        {
            if (val > 0) missingErrors += val;
            if (val < 0) extraErrors -= val;
        }
        outOfOrderErrors = GetOutOfOrderElements(desiredIngredients, actualIngredients);

        float accuracy = 100 - (extraErrors * extraPenalty) - (missingErrors * missingPenalty) - (outOfOrderErrors * outOfOrderPenalty);
        if (accuracy < 0) accuracy = 0;
        Debug.Log("AccuracyScore=" + accuracy + ", missing=" + missingErrors + ", extra=" + extraErrors + ", outOfOrder=" + outOfOrderErrors);
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

    public virtual float CalcSpeedScore(string[] desiredIngredients, float timeSinceOrderStarted)
    {
        if (desiredIngredients.Length == 0) return 100;
        float expectedTime = baseSecondsForBurger + secondsPerIngredient * desiredIngredients.Length - LevelManager.Instance.settings.difficultyLevel;
        float speedPenalty = 50 * (timeSinceOrderStarted / expectedTime) - 25;
        if (speedPenalty > 100) speedPenalty = 100;
        if (speedPenalty < 0) speedPenalty = 0;
        Debug.Log("SpeedScore=" + (100 - speedPenalty) + ", timesinceOrderStarted=" + timeSinceOrderStarted + ", expectedTime=" + expectedTime);
        return 100 - speedPenalty;
    }

}

