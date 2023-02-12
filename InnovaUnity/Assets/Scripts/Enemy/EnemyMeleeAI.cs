using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMeleeAI : BaseCharacter
{
    public bool hasTakenThunderClap = false;
    public int listLocation;
    public List<EnemyAI> enemyList;

    Transform player;
    NavMeshAgent navMeshAgent;

    [SerializeField] Essence essencePrefab;
    [SerializeField] GameObject sword;
    [SerializeField] LayerMask layer;
    [SerializeField] float searchRadius = 90f;
    [SerializeField] float attackRange = 10f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] private bool attacked = false;
    [SerializeField] private bool takedamage = false;
    public Image hpBar;
    enum Enemy { walking, running, attacking, reposition, dead };

    private Enemy action = Enemy.walking;

    Animator animEnemyMelee;

    int posture = 1;

    Vector3 newPosition;
    void Die()
    {
        if (hasTakenThunderClap)
        {
            enemyList.RemoveAt(listLocation);
        }

        enemyValue = Random.Range(10, 21) * (1 + (MainGame.instance.gameDifficulty * 0.1f));
        int value = (int)enemyValue;
        Essence essence = Instantiate(essencePrefab, new Vector3(transform.position.x, 0.1f, transform.position.z), transform.rotation);
        essence.value = value;
    }

    public override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = MainGame.instance.playerCharacter.transform;
        animEnemyMelee = GetComponentInChildren<Animator>();
    }

    public override void Update()
    {
        hpBar.fillAmount = hp / maxHpCurrent;
        Debug.Log(hp / maxHpCurrent);
        float distance = Vector3.Distance(player.position, transform.position);

        //if (action != Enemy.shooting)
        //{
        //    MainGame.instance.gunProjectile.StopShoot();
        //}

        if (action == Enemy.walking)
        {
            animEnemyMelee.SetBool("Running",true);
            navMeshAgent.speed = 3.5f;
            navMeshAgent.SetDestination(player.position);

            Vector3 dir = player.position - transform.position;

            if (distance <= searchRadius)
            {

                RaycastHit hit;
                Ray ray = new Ray(transform.position, MainGame.instance.playerCharacter.transform.position - transform.position);

                if (Physics.Raycast(ray, out hit, searchRadius, layer))
                {

                    if (hit.transform.tag == "Player")
                    {

                        action = Enemy.running;
                    }
                }
            }
        }

        if (action == Enemy.running)
        {
            animEnemyMelee.SetBool("Running", true);
            navMeshAgent.speed = 7;
            navMeshAgent.SetDestination(player.position);
            Vector3 dir = player.position - transform.position;

            if (distance <= attackRange)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, dir, out hit, attackRange, layer))
                {
                    if (hit.transform.tag == "Player")
                    {
                        action = Enemy.attacking;
                    }
                    else
                    {
                        action = Enemy.walking;
                    }
                }
            }

        }

        if (action == Enemy.attacking)
        {
            animEnemyMelee.SetBool("Running", false);
            if (distance > attackRange)
            {
                action = Enemy.running;
                return;
            }
            navMeshAgent.SetDestination(transform.position);
            if (!attacked)
            {
                Attack();
            }
            //MainGame.instance.gunProjectile.GunShoot();

            //if (isHit)
            //{
            //    action = Enemy.reposition;
            //}
        }

        //if (action == Enemy.reposition)
        //{
        //    StartCoroutine(Reposition());
        //}

        if (action == Enemy.dead)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Attack()
    {
        base.Attack();
        faceTarget();
        StartCoroutine(animAttack());
    }

    IEnumerator animAttack ()
    {
        attacked = true;
        if (animEnemyMelee.GetInteger("Attack") == 1)
        {
            posture = 2;
        }
        else
        {
            posture = 1;
        }
        animEnemyMelee.SetInteger("Attack", posture);
        yield return new WaitForSeconds(0.5f);
        takedamage = true;
        if (animEnemyMelee.GetInteger("Attack") == 1)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }
        attacked = false;
        Animator anim = sword.GetComponent<Animator>();
        anim.SetTrigger("Attack");
    }

    private void faceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Collider enemyCollider = GetComponent<Collider>();
        //Physics.IgnoreCollision(other, enemyCollider, false);
        if (other.gameObject.tag == "Player" && takedamage)
        {
            float dmg = attackPower;
            MainGame.instance.playerCharacter.TakeDamage(dmg);
            takedamage = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider enemyCollider = GetComponent<Collider>();
     if (collision.transform.tag == "Player")
        {
            Rigidbody enemyRbdy = GetComponent<Rigidbody>();
            enemyRbdy.velocity = Vector2.zero;
            Physics.IgnoreCollision(collision.collider, enemyCollider);
        }
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        if (hp <= 0)
        {
            action = Enemy.dead;
            Die();
        }
    }
}
