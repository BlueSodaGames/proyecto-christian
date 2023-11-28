using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Loot
{
    public Powerup thisLoot;
    public int lootChance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{

    public Loot[] loots;
    public int numberOfThis;
    public Powerup LootPowerup()
    {
        int acumulativeProb = 0;
        int currentProb = Random.Range(0, 100);
        for (int i = 0; i < loots.Length; i++)
        {
            acumulativeProb += loots[i].lootChance;
            if (currentProb <= acumulativeProb)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
