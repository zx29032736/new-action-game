using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatLevelRamp : MonoBehaviour {

    public List<int> playerLevelupRequiredExp = new List<int>();

    public static Dictionary<int, int> CharacterLevelRamp = new Dictionary<int, int>();

    private void Awake()
    {
        for(int i = 0; i < playerLevelupRequiredExp.Count; i++)
        {
            CharacterLevelRamp.Add(i, playerLevelupRequiredExp[i]);
        }
    }
}
