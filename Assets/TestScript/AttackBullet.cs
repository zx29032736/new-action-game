using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStat
{
    public int baseDamage = 1;
    public int baseMaxDamage = 1;
    [System.NonSerialized]
    public int characterAttackPoint = 1;
    public int totalDamage;

    public int floatingDamage = 10;

    public GameObject attacker;

    public bool isFlinch = true;
}

public class AttackBullet : MonoBehaviour {

    public float duration = 1f;
    public float speed = 20f;
    public Vector3 dirction = Vector3.forward;

    public BulletStat bulletStat;
	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, duration);
	}

    private void Update()
    {
        Vector3 dir = transform.rotation * dirction;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void SetupBullet(GameObject owner, int physicDmg, int spcialDmg)
    {
        bulletStat.attacker = owner;
        bulletStat.characterAttackPoint = physicDmg;

        int min = 100 - bulletStat.floatingDamage;
        int max = 100 + bulletStat.floatingDamage;
        int randomDmg = UnityEngine.Random.Range(bulletStat.baseDamage, bulletStat.baseMaxDamage);

        bulletStat.totalDamage = (randomDmg + bulletStat.characterAttackPoint) * UnityEngine.Random.Range(min, max) / 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        //prevent attack to self or teammates
       if(other.CompareTag("Enemy") && bulletStat.attacker.CompareTag("Player"))
        {
            CharacterStat stat = other.GetComponent<CharacterStat>();
            if (stat != null)
            {
                stat.OnDamage(bulletStat.totalDamage);
            }
            if (bulletStat.isFlinch)
            {
                other.SendMessage("Flinch", transform.rotation * dirction, SendMessageOptions.DontRequireReceiver);
            }
        }
       else if(other.CompareTag("Player") && bulletStat.attacker.CompareTag("Enemy"))
        {
            CharacterStat stat = other.GetComponent<CharacterStat>();
            if (stat != null)
            {
                stat.OnDamage(bulletStat.totalDamage);
            }
            if (bulletStat.isFlinch)
            {
                other.SendMessage("Flinch", transform.rotation * dirction, SendMessageOptions.DontRequireReceiver);
            }
        }


        Destroy(gameObject);
    }
}
