using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger_RangeEnemy : MonoBehaviour
{

    public EnemyAI enemy;
    private void Start()
    {
        enemy = transform.parent.GetComponent<EnemyAI>();
    }
    public void ShootTrigger()
    {
        Debug.Log("EnemyAi Shoot");
        enemy.gunProjectile.NewShoot();
    }
}
