using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour {

    public string characterName;

    public Stat attack;
    public Stat defense;
    public Stat spAttack;
    public Stat spDefense;
    public Stat speed;

    public int maxHealth = 100;
    //[HideInInspector]
    public int currentHealth = 0;

    public int Maxmana = 100;
    //[HideInInspector]
    public int currentMana = 0;

    public bool isDead = false;

    public void OnDamage(int dmg)
    {
        if (!isDead)
        {
            int finalDmg = dmg - defense.GetValue();

            if (finalDmg <= 0)
                finalDmg = 1;

            currentHealth -= finalDmg;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " is died.");
        isDead = true;
    }

    public void SetUpStat()
    {
        currentHealth = maxHealth;
        currentMana = Maxmana;
    }
}
