using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour {

    public Text characterNameText;
    public Text hpText;
    public Text mpText;
    public Text levelText;
    public Text attackText;
    public Text defenseText;
    public Text spAttackText;
    public Text spDefenseText;
    public Text speedText;
    public Text expText;

    public Image icon;
    public Text descriptionText;

    public Dropdown dropdown;
    public EnemyStat[] enemyStat;
    PlayerStat playerStat;

    private void OnEnable()
    {
        EquipmentController.onEquipmentChanged += OnEquipmentChanged;  
        SetUpPanel();
    }

    private void OnDisable()
    {
        EquipmentController.onEquipmentChanged -= OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment old , Equipment newE)
    {
        dropdown.value = 0;
        SetupPlayerValue();
    }


    void SetUpPanel()
    {
        if (playerStat == null)
            playerStat = FindObjectOfType<PlayerStat>();

        dropdown.value = 0;
        if(dropdown.options.Count == 0)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = playerStat.characterName + " (Player)" });

            for (int i = 0; i < enemyStat.Length; i++)
            {
                dropdown.options.Add(new Dropdown.OptionData() { text = enemyStat[i].characterName + " (Enemy)" });
            }
        }

        SetupPlayerValue();
    }

    void SetupPlayerValue()
    {
        if (playerStat == null)
            return;
        characterNameText.text = playerStat.characterName;
        hpText.text = playerStat.maxHealth.ToString();
        mpText.text = playerStat.Maxmana.ToString();
        attackText.text = playerStat.attack.GetAdjsutedValue().ToString();
        defenseText.text = playerStat.defense.GetAdjsutedValue().ToString();
        spAttackText.text = playerStat.spAttack.GetAdjsutedValue().ToString();
        spDefenseText.text = playerStat.spDefense.GetAdjsutedValue().ToString();
        speedText.text = playerStat.speed.GetAdjsutedValue().ToString();

        // maybe have been modified in PlayerStat and jsut take current exp
        int nextLevel = playerStat.level; //+ 1;
        if (PlayerStatLevelRamp.CharacterLevelRamp.ContainsKey(nextLevel))
        {
            expText.text = playerStat.currentExp + " / " + PlayerStatLevelRamp.CharacterLevelRamp[nextLevel];
            levelText.text = playerStat.level.ToString();

        }
        else
        {
            expText.text = nextLevel.ToString();//playerStat.currentExp + " / " + PlayerStatLevelRamp.CharacterLevelRamp[nextLevel];
            levelText.text = playerStat.level.ToString() + " (max)";
        }

        icon.overrideSprite = playerStat.icon;
        descriptionText.text = playerStat.description;
    }

    void SetupEnemyValue(int index)
    {
        characterNameText.text = enemyStat[index].characterName;
        hpText.text = enemyStat[index].maxHealth.ToString();
        mpText.text = enemyStat[index].Maxmana.ToString();
        attackText.text = enemyStat[index].attack.GetValue().ToString();
        defenseText.text = enemyStat[index].defense.GetValue().ToString();
        spAttackText.text = enemyStat[index].spAttack.GetValue().ToString();
        spDefenseText.text = enemyStat[index].spDefense.GetValue().ToString();
        speedText.text = enemyStat[index].speed.GetValue().ToString();

        levelText.text = "no setting";
        expText.text = "no setting";


        icon.overrideSprite = enemyStat[index].icon;
        descriptionText.text = enemyStat[index].description;
    }

    public void OnDropdownValueChanged()
    {
        int index = dropdown.value;


        if (index == 0)
            SetupPlayerValue();
        else
            SetupEnemyValue(index-1);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
