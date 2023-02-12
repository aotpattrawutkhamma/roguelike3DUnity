using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter : MonoBehaviour
{
    public float hp;
    public float maxHpCurrent;
    public float maxHpBase;
    public float attackPower;
    public float defCurrent;
    public float defBase;

    public float sleepTimer;
    protected float i;
    public float enemyValue;

    public virtual void Start()
    {
      
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(sleepTimer > 0)
        {
            sleepTimer -= Time.deltaTime;
        }
        else if(sleepTimer < 0)
        {
            sleepTimer = 0;
            Sleep_WakeUp();
        }

    }

    public virtual void TakeDamage(float dmg)
    {
        hp -= dmg * (100 / (100 + defCurrent));
    }
    public virtual void Attack()
    {

    }

    public virtual void TakeSleep(float timer)
    {
        sleepTimer = timer;
    }
    public virtual void Sleep_WakeUp()
    {

    }
}
