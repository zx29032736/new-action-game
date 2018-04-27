using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiController : MonoBehaviour {

    public Text playerNameText;
    public Image healthImg;
    public Image manaImg;
    public Text healthText;
    public Text manaText;

    public Image expImg;
    public Text levelText;

    PlayerStat playerStat;

    public void SetupUi(PlayerStat ps)
    {
        playerStat = ps;
        playerNameText.text = playerStat.characterName;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (playerStat == null)
            return;

        healthText.text = playerStat.currentHealth + " / " + playerStat.maxHealth;
        float healthRatio = Mathf.Clamp01((float)(playerStat.maxHealth - playerStat.currentHealth) / (float)playerStat.maxHealth);
        healthImg.fillAmount = 1 - healthRatio;

        manaText.text = playerStat.currentMana + " / " + playerStat.Maxmana;
        float manaRatio = Mathf.Clamp01((float)(playerStat.Maxmana - playerStat.currentMana) / (float)playerStat.Maxmana);
        manaImg.fillAmount = 1 - manaRatio;

        float expRatio = Mathf.Clamp01((float)(playerStat.nextLevelExp - playerStat.currentExp) / (float)playerStat.nextLevelExp);
        expImg.fillAmount = 1- expRatio;

        levelText.text = "lv : " + playerStat.level.ToString();
    }
}
