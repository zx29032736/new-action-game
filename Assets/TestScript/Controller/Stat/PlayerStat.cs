using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat {

    public int currentExp { get; private set; }
    public int level { get; private set; }

    public int nextLevelExp = 0;

    public Transform bornPos;

    public string description;
    public Sprite icon;

    private void OnEnable()
    {
        EquipmentController.onEquipmentChanged += onEquipmentChanged;
    }

    private void OnDisable()
    {
        EquipmentController.onEquipmentChanged -= onEquipmentChanged;
    }

    void onEquipmentChanged(Equipment old, Equipment newItem)
    {
        if (newItem != null)
        {
            attack.AddModifier(newItem.attack);
            spAttack.AddModifier(newItem.spAttack);
            defense.AddModifier(newItem.defense);
            spDefense.AddModifier(newItem.spDefense);
            speed.AddModifier(newItem.speed);
        }

        if (old != null)
        {
            attack.RemoveModifier(old.attack);
            spAttack.RemoveModifier(old.spAttack);
            defense.RemoveModifier(old.defense);
            spDefense.RemoveModifier(old.spDefense);
            speed.RemoveModifier(old.speed);
        }
    }

    private void Start()
    {
        SetUpStat();

        //set up ui
        GameplayManager.instance.screenUiController.playerUiController.SetupUi(this);
        //
        currentExp = 0;
        level = 1;
        nextLevelExp = PlayerStatLevelRamp.CharacterLevelRamp[level];
    }

    public void AddExp(int value)
    {
        currentExp += value;
        CheckLevelUp();
        //Debug.Log(string.Format("current exp : {0}, level : {1}",currentExp,level));
    }

    void CheckLevelUp()
    {
        if (!PlayerStatLevelRamp.CharacterLevelRamp.ContainsKey(level))
            return;

        while (currentExp >= PlayerStatLevelRamp.CharacterLevelRamp[level])
        {
            currentExp -= PlayerStatLevelRamp.CharacterLevelRamp[level];
            level++;

            if (!PlayerStatLevelRamp.CharacterLevelRamp.ContainsKey(level))
            {
                currentExp = 0;
                break;
            }
        }
        nextLevelExp = PlayerStatLevelRamp.CharacterLevelRamp[level];
    }
    
    public void Revive()
    {
        Vector3 v3= Vector3.zero;

        if (bornPos != null)
            v3 = bornPos.position;

        isDead = false;
        currentHealth = maxHealth;
        currentMana = Maxmana;

        transform.position = v3;
    }

    public override void Die()
    {
        base.Die();
        GameplayManager.instance.OnOffSettingPanel();
    }
}
