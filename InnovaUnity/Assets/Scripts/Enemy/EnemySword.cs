using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    EnemyMeleeAI enemyMeleeAI;

    public void Start()
    {
        enemyMeleeAI = gameObject.transform.GetComponentInParent<EnemyMeleeAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float dmg = enemyMeleeAI.attackPower;
            MainGame.instance.playerCharacter.TakeDamage(dmg);
        }
    }
}
