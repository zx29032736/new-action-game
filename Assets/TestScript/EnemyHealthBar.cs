using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {

    public float maxVisiableDistance = 50;

    public Image healthImg;
    public Text nameText;

    EnemyStat enemyStat;
    Transform enemyT;
    Transform playerT;

    public  void SetupHealthBar(EnemyStat eStat,Transform player)
    {
        enemyStat = eStat;
        enemyT = eStat.transform;
        playerT = player;
        nameText.text = eStat.characterName;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (playerT == null)
            return;

        float distance = (playerT.position - enemyT.position).magnitude;
        float a = Mathf.Clamp01((maxVisiableDistance - distance) / maxVisiableDistance);
        transform.localScale = new Vector3(1, 1, 1) * a;

        GameplayManager.instance.screenUiController.UpdatingNpcUiPos(transform, enemyT);

        float healthRatio = Mathf.Clamp01((float)(enemyStat.maxHealth - enemyStat.currentHealth) / (float)enemyStat.maxHealth);
        healthImg.fillAmount = 1 - Mathf.Abs( healthRatio);

        if (!enemyStat.gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
