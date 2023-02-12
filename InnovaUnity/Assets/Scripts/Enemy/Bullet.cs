using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    EnemyAI enemyAi;
    bool isInit = false;
    Vector3 direction;
    float shootForce;
    public void Initiate(Vector3 dir, float speed)
    {
        //direction = dir;
        shootForce = speed;
        bulletPos();
    }
    public void Ownership(EnemyAI enemy)
    {
        enemyAi = enemy;
    }

    private void Update()
    {
        if(isInit)
        {
            transform.Translate(direction * Time.deltaTime, Space.World);
        }
    }


    private void bulletPos ()
    {
        direction = (MainGame.instance.playerCharacter.transform.position - transform.position).normalized * shootForce;
        isInit = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyAi.HitPlayer();
            float dmg = enemyAi.attackPower;
            MainGame.instance.playerCharacter.TakeDamage(dmg);
        }
        Destroy(this.gameObject);
    }
}
