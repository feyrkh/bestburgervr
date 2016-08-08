using UnityEngine;
using System.Collections.Generic;

public class CompositeOrderRules : OrderRules
{
    public string[] ingredientTypes = new string[]
    {
        // 0: base of the burger
        "meat",
        // 1: one vegetable
        "lettuce:tomato",
        // 2: two vegetables
        "lettuce,tomato:tomato,lettuce",
        // 3: extra condiments
        "mustard:ketchup",
        // 4: bottom bun
        "bottom_bun",
        // 5: top bun
        "top_bun",
        // 6: meat, optional condiments
        "meat:ketchup,meat:mustard,meat:ketchup,ketchup,meat:mustard,mustard,meat"
    };
    private string[] defaultCombinations = { "0" };
    public string[][] difficultyLevelCombinations =
    {
        // Difficulty level 1
        new string[] {
            "4,0,5",
            "4,0,5",
            "4,0,1,5"
        },
        // Difficulty level 2
        new string[] {
            "4,0,1,5",
            "4,0,1,3,5",
            "4,0,2,5",
        },
        // Difficulty level 3
        new string[] {
            "4,0,2,5",
            "4,6,2,5",
            "4,0,3,1,5",
            "4,6,3,2,5"
        },
        // Difficulty level 4
        new string[] {
            "4,6,3,2,5",
            "4,6,1,4,0,1,5",
            "4,6,0,1,1,3,5",
            "4,6,1,1,4,0,3,5"
        },
        // Difficulty level 5
        new string[] {
            "4,6,1,4,0,1,5",
            "4,6,0,1,1,3,5",
            "4,6,1,1,4,0,3,5",
            "4,6,3,4,0,3,4,0,3,5",
            "4,6,1,4,0,1,4,0,1,5"
        },
    };

    public override void GenerateOrder(NpcOrder npc)
    {
        List<string> newOrder = new List<string>();
        string[] combinations = null;
        int difficultyLevel = LevelManager.Instance.settings.difficultyLevel;
        // Get the set of possible burger definitions for the current difficulty level
        while(difficultyLevel > 0 && (combinations == null || combinations.Length == 0))
        {
            difficultyLevel--;
            if (difficultyLevel < difficultyLevelCombinations.Length)
            {
                combinations = difficultyLevelCombinations[difficultyLevel];
            } 
        }
        // If we had bad input (missing/empty burger definition) just use the default
        if (combinations == null) combinations = defaultCombinations;
        // Pick a random burger definition from the current difficulty level list
        string[] orderCombo = combinations[Random.Range(0, combinations.Length)].Split(',');
        for(int i=0;i<orderCombo.Length;i++)
        {
            int ingredientTypeIdx = int.Parse(orderCombo[i]);
            // Get a random ingredient list for the current burger definition component
            string[] ingredientOptions = ingredientTypes[ingredientTypeIdx].Split(':');
            // Add a random entry from the ingredient list
            newOrder.AddRange(ingredientOptions[Random.Range(0,ingredientOptions.Length)].Split(','));
        }
        for (int i = 0; i < newOrder.Count; i++)
        {
            Debug.Log("Generated order: " + newOrder[i], npc);
        }
        npc.SetOrder(newOrder.ToArray());
    }
}
