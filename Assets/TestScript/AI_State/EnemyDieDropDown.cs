using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDorp
{
    public string dropName;
    public PickupItem item;
    [Range(0,100)]
    public int chance = 0;
}

public class EnemyDieDropDown : MonoBehaviour {

    public EnemyDorp[] dropDown;
    public int gainGold = 0;
    public int gainExp = 0;
    GameObject player;
    PlayerStat playerStat;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerStat = player.GetComponent<PlayerStat>();
    }

    public void Init()
    {
        for(int i = 0; i < dropDown.Length; i++)
        {
            int rng = UnityEngine.Random.Range(0, 101);

            if(rng < dropDown[i].chance)
            {
                PickupItem go = Instantiate<PickupItem>(dropDown[i].item, new Vector3(transform.position.x, 3, transform.position.z),Quaternion.identity);
                go.SetupItem(dropDown[i].dropName);
                //Debug.Log(go.transform.position);
            }
        }
        playerStat.AddExp(gainExp);
    }
}
