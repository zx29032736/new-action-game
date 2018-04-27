using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour {

    public InputField playerNameField;
    public Scrollbar bgmVolumeScrollbar;
    public Scrollbar fxVolumeScrollbar;

    public Button saveButton;
    public Button cancelButton;

    public PlayerStat playerStat;

    private void OnEnable()
    {
        if (playerStat == null)
            playerStat = FindObjectOfType<PlayerStat>();

        Init();
    }

    public void Init()
    {
        playerNameField.text = PlayerPrefs.GetString("PlayerName", playerStat.characterName);
        bgmVolumeScrollbar.value = PlayerPrefs.GetFloat("BgmVolume", 0);
        fxVolumeScrollbar.value = PlayerPrefs.GetFloat("FxVolume", 0);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("PlayerName", playerNameField.text);
        PlayerPrefs.SetFloat("BgmVolume", bgmVolumeScrollbar.value);
        PlayerPrefs.SetFloat("FxVolume", fxVolumeScrollbar.value);

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    // for test

    public EnemyAI[] allEnemies = new EnemyAI[1];

    public void ReviveAllEnemy()
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].ReLive();
        }
    }

    public void RevivePlayer()
    {
        playerStat.Revive();
    }
}
