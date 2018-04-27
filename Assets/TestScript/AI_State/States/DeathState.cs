using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State {

    public override void PreUpdate(AI ai)
    {
        if (ai.stat.isDead)
        {
            ai.StartCoroutine(Dead(ai));
        }
        ((EnemyAI)ai).enemyDieDropDown.Init();
        ai.characterController.enabled = false;
        GameplayManager.instance.questController.UpdateQuestProgress(ai.name);
        GameplayManager.instance.screenUiController.ShowStringOnScreen(string.Format("<color=red>{0}</color> is killed!", ai.name));
    }

    public override void Update(AI ai)
    {
        
    }

    public override void Exit(AI ai)
    {
        //ai.characterController.enabled = true;
    }

    IEnumerator Dead(AI ai)
    {
        ai.animator.SetBool("die", true);
        yield return new WaitForSeconds(2f);
       
        float timer = 0;

        while(timer < 1.5f)
        {
            ai.transform.position += Vector3.down * 0.05f * timer / 1;
            timer += Time.deltaTime;
            yield return null;
        }

        ai.gameObject.SetActive(false);
    }
}
