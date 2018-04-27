using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplaySlot : MonoBehaviour {

    public Button myButton;
    public Button cancelButton;
    public Button rewardButton;
    public Text questNameText;

    public QuestController questController;
    public Quest myQuest;

    private void Awake()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => { questController.CancelQuest(myQuest.questName); });
        cancelButton.gameObject.SetActive(true);

        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() => 
        {
            if (questController.QuestCompeletedReward(myQuest))
                rewardButton.interactable = false;
        });

        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() => { questController.OnSelectQuest(this); });

        rewardButton.gameObject.SetActive(false);
    }

    public void SetupSlot(Quest q)
    {
        myQuest = q;
        questNameText.text = myQuest.questName;

        gameObject.SetActive(true);
    }

    public void OnQutestCompeleted()
    {
        rewardButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
    }

    public void ClearSlot()
    {
        myQuest = null;
        gameObject.SetActive(false);
    }
}
