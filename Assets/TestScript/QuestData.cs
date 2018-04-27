using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillEnemyQuestCondition
{
    public string requiredMonsterName;
    public int requiredCount;
    //[System.NonSerialized]
    public int currentCount = 0;
}

[System.Serializable]
public class HandInItemQuestCondition
{
    public string requiredItemName;
    public int requiedCount;
    //[System.NonSerialized]
    public int currentCount = 0;
}

[System.Serializable]
public class Quest
{
    public string questName;
    public int questID;
    [System.NonSerialized]
    public bool isCompeleted = false;
    [System.NonSerialized]
    public bool isRewarded = false;
    public string questDescription;

    public KillEnemyQuestCondition[] killEnemyQuestConditions;
    public HandInItemQuestCondition[] handInItemQuestConditions;

    public int rewardExp;
    public int rewardGold;
    public string[] rewarnItemName;
}

public class QuestData : MonoBehaviour {

    public List<Quest> quests = new List<Quest>();
    public static Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();

    private void Awake()
    {
        foreach (var quest in quests)
        {
            questDictionary.Add(quest.questName, quest);
        }
    }
}
