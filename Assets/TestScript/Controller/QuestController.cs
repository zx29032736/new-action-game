using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {

    public List<Quest> myQuest = new List<Quest>();

    public QuestDisplaySlot[] questDisplaySlots;
    QuestDisplaySlot selectedQuest = null;

    public delegate void OnQuestStateChanged(bool questEnable, Quest quest);
    public static event OnQuestStateChanged onQuestStateChanged;

    //detail panel
    public GameObject detailParent;
    public Text title;
    public Text detail;
    public Text description;
    //
    private void OnEnable()
    {
        RefreshQuest();
        //onQuestStateChanged += OnQuestChanged;
    }

    private void OnDisable()
    {
        //onQuestStateChanged -= OnQuestChanged;
    }

    void OnQuestChanged()
    {

    }

    public void RefreshQuest()
    {
        foreach (var slot in questDisplaySlots)
        {
            slot.ClearSlot();
        }

        for (int i = 0; i < myQuest.Count; i++)
        {
            questDisplaySlots[i].SetupSlot(myQuest[i]);
            bool isCompeleted = CheckQuestCompeleted(myQuest[i]);

            if (isCompeleted)
                questDisplaySlots[i].OnQutestCompeleted();
        }

        detailParent.SetActive(false);
        selectedQuest = null;
    }

    public void AddQuest(string questName)
    {
        if(!QuestData.questDictionary.ContainsKey(questName))
        {
            Debug.LogError(questName +" quest is not exist in data, please to check..");
            return;
        }

        Quest q = QuestData.questDictionary[questName];
        if (!myQuest.Exists( x => x == q))
        {
            myQuest.Add(q);
        }
        else
        {
            Debug.LogError("quest is already exist");
        }

        RefreshQuest();

        if (onQuestStateChanged != null)
            onQuestStateChanged(false,q);
    }

    public void CancelQuest(string questName)
    {
        Quest q = QuestData.questDictionary[questName];
        if (myQuest.Exists(x => x == q))
        {
            myQuest.Remove(q);
        }
        else
        {
            Debug.LogError("quest is not exist");
        }

        RefreshQuest();

        if (onQuestStateChanged != null)
            onQuestStateChanged(true,q);
    }

    public bool ExistInMyQuest(string questName)
    {
        return myQuest.Exists(x => x.questName.Contains(questName));
    }

    public bool CheckQuestCompeleted(Quest quest)
    {
        if (quest.isCompeleted)
            return true;
        
        bool enemyKilledCompeleted = true;
        bool handInItemCompeleted = true;

        for (int j = 0; j < quest.killEnemyQuestConditions.Length; j++)
        {
            if (quest.killEnemyQuestConditions[j].currentCount >= quest.killEnemyQuestConditions[j].requiredCount)
            {
                enemyKilledCompeleted = true;
            }
            else
            {
                enemyKilledCompeleted = false;
                //Debug.Log(" killEnemyQuestConditions is not success");
                break;
            }
        }

        // prevent that items are removed and quest is still uncompeleted.
        if (enemyKilledCompeleted == false)
            return false;

        for (int j = 0; j < quest.handInItemQuestConditions.Length; j++)
        {
            quest.handInItemQuestConditions[j].currentCount = GameplayManager.instance.inventoryController.FindItemCount(quest.handInItemQuestConditions[j].requiredItemName);
            Debug.Log(quest.handInItemQuestConditions[j].currentCount + " current count");
            if (quest.handInItemQuestConditions[j].currentCount >= quest.handInItemQuestConditions[j].requiedCount)
            {
                handInItemCompeleted = true;
            }
            else
            {
                handInItemCompeleted = false;
                Debug.Log(" handInItemCompeleted is not success");

                break;
            }

            if (j == quest.handInItemQuestConditions.Length - 1)
            {
                // inventory remove item
                Debug.Log(" item deleted");
                handInItemCompeleted = GameplayManager.instance.inventoryController.DeleteItem(quest.handInItemQuestConditions[j].requiredItemName, quest.handInItemQuestConditions[j].requiedCount);
            }
        }

        if (enemyKilledCompeleted && handInItemCompeleted)
        {
            quest.isCompeleted = true;
            Debug.Log( " quest : " + quest.questName + " is successed");
            return true;
        }
        return false;
        
    }

    public bool QuestCompeletedReward(Quest quest)
    {
        if (!quest.isRewarded && quest.isCompeleted)
        {
            for(int i = 0; i < quest.rewarnItemName.Length; i++)
            {
                // add item to inventory
                // if inventory dont have enough storage => return
                // maybe invetory add "check leave count" fuction to check is having enough count to store
                Item item = ItemData.CreateItem(quest.rewarnItemName[i]);
                if (!GameplayManager.instance.inventoryController.AddItemToInventory(item))
                {
                    return false;
                }
                
            }

            PlayerStat playerStat = FindObjectOfType<PlayerStat>();
            playerStat.AddExp(quest.rewardExp);

            //add gold

            quest.isRewarded = true;
            return true;
        }
        return false;
    }

    public void UpdateQuestProgress(string enemyName)
    {
        for(int i = 0; i < myQuest.Count; i++)
        {
            if (myQuest[i].isCompeleted)
                continue;

            for(int j =0; j < myQuest[i].killEnemyQuestConditions.Length; j++)
            {
                if (myQuest[i].killEnemyQuestConditions[j].requiredMonsterName.Contains(enemyName))
                {
                    // required enemy is match with input
                    // add to the current coount
                    // if current count > required count =>  current count = required count

                    Debug.Log(enemyName + " killed value in " + myQuest[i].questName + " is updated");

                    myQuest[i].killEnemyQuestConditions[j].currentCount++;

                    if (myQuest[i].killEnemyQuestConditions[j].currentCount >= myQuest[i].killEnemyQuestConditions[j].requiredCount)
                        myQuest[i].killEnemyQuestConditions[j].currentCount = myQuest[i].killEnemyQuestConditions[j].requiredCount;
                }
            }
        }
        RefreshQuest();
    }

    public void OnSelectQuest(QuestDisplaySlot slot)
    {
        if (selectedQuest == slot)
        {
            selectedQuest = null;
            detailParent.SetActive(false);
            return;
        }

        selectedQuest = slot;
        title.text = selectedQuest.myQuest.questName + " Details ";
        description.text = selectedQuest.myQuest.questDescription;

        string s = "";
        s += "<size=35><color=green>Is Compeleted : </color></size>\n" + selectedQuest.myQuest.isCompeleted + "\n";

        s += "<size=35><color=green>Kill The Monster : </color></size>\n";
        for (int i = 0; i < selectedQuest.myQuest.killEnemyQuestConditions.Length; i++)
        {
            s += selectedQuest.myQuest.killEnemyQuestConditions[i].requiredMonsterName + " " + selectedQuest.myQuest.killEnemyQuestConditions[i].currentCount + " / " + selectedQuest.myQuest.killEnemyQuestConditions[i].requiredCount + "\n";
        }

        s += "<size=35><color=green>Hand in Item : </color></size>\n";
        for (int i = 0; i < selectedQuest.myQuest.handInItemQuestConditions.Length; i++)
        {
            s += string.Format("{0} for {1} counts \n", selectedQuest.myQuest.handInItemQuestConditions[i].requiredItemName, selectedQuest.myQuest.handInItemQuestConditions[i].requiedCount);
        }

        s += "<size=35><color=green>Reward : </color></size>\n";
        s += string.Format("Gold : {0} \nExp : {1} \n", selectedQuest.myQuest.rewardGold, selectedQuest.myQuest.rewardExp);

        for(int i = 0; i < selectedQuest.myQuest.rewarnItemName.Length; i++)
        {
            s += string.Format("Item : {0} \n", selectedQuest.myQuest.rewarnItemName[i]);
        }

        detail.text = s;
        detailParent.SetActive(true);
    }

    public void ClosePanel()
    {
        foreach(var slot in questDisplaySlots)
        {
            slot.ClearSlot();
        }

        gameObject.SetActive(false);
    }
}
