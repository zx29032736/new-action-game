using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
    private int baseValue;
    //[SerializeField]
    private int adjustedValue;

    private List<int> modifiers = new List<int>();
    private List<int> buffs = new List<int>();

    public int GetValue()
    {
        return baseValue;
    }

    public int GetAdjsutedValue()
    {
        int adjValue = baseValue;

        foreach (var v in modifiers)
            adjValue += v;

        foreach(var v in buffs)
            adjValue += v;

        adjustedValue = adjValue;

        return adjValue;
    }

    public void AddModifier(int value)
    {
        if(value != 0)
        {
            modifiers.Add(value);
        }
    }

    public void RemoveModifier(int value)
    {
        if(modifiers.Contains(value))
        {
            modifiers.Remove(value);
        }
    }

    public void AddBuff()
    {

    }

    public void RemoveBuff()
    {

    }

}
