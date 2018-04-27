using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcNameUI : MonoBehaviour {

    public float maxVisiableDistance = 50;

    public Text nameText;
    Transform myNpc;
    Transform player;

    public void SetUpUI(string name, Transform npc, Transform playerTr)
    {
        nameText.text = name;
        myNpc = npc;
        player = playerTr;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = (player.position - myNpc.transform.position).magnitude;
        //min 5,max 50
        float a = Mathf.Clamp01( (maxVisiableDistance - distance) / maxVisiableDistance);
        transform.localScale = new Vector3(1, 1, 1) * a;

        GameplayManager.instance.screenUiController.UpdatingNpcUiPos(transform, myNpc);//
    }
}
