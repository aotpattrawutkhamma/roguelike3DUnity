using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : BaseCharacter
{
    bool setTriggerAttack = false;
    public GunProjectile gunProjectile;
    public bool hasTakenThunderClap = false;
    public int listLocation;
    public List<EnemyAI> enemyList;
    Animator anim;
    Animation animation;

    Transform player;
    NavMeshAgent navMeshAgent;
    public Essence essencePrefab;

    [SerializeField] LayerMask layer;
    [SerializeField] float searchRadius = 90f;
    [SerializeField] float shootRange = 50f;
    [SerializeField] float turnSpeed = 5f;

    public Image hpBar;
    enum Enemy { walking, running, shooting, reposition, dead };

    private Enemy action = Enemy.walking;
    bool isHit = false;
    bool changingPosition = false;

    Vector3 newPosition;
    void Die()
    {
        if (hasTakenThunderClap)
        {
            enemyList.RemoveAt(listLocation);
        }

        enemyValue = Random.Range(10, 21) * (1 + (MainGame.instance.gameDifficulty * 0.1f));
        int value = (int)enemyValue;
        Essence essence = Instantiate(essencePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
        essence.value = value;
        //Destroy(this.gameObject);
    }
    public override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = MainGame.instance.playerCharacter.transform;
        gunProjectile = gameObject.GetComponent<GunProjectile>();
        maxHpCurrent = maxHpBase;
        anim = GetComponentInChildren<Animator>();
        animation = GetComponentInChildren<Animation>();
    }

    public override void Update()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (hp / maxHpCurrent);
          
        }
        if (hp <= 0)
        {
            Die();
        }
        float distance = Vector3.Distance(player.position, transform.position);

        if (action != Enemy.shooting)
        {
            gunProjectile.StopShoot();
        }

        if (action == Enemy.walking)
        {
            anim.ResetTrigger("Attack");
            setTriggerAttack = false;
            anim.SetBool("Running", true);
            changingPosition = false;
            //isHit = false;
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
            anim.ResetTrigger("Attack");
            setTriggerAttack = false;
            anim.SetBool("Running", true);
            changingPosition = false;
            //isHit = false;
            navMeshAgent.speed = 5;
            navMeshAgent.SetDestination(player.position);
            Vector3 dir = player.position - transform.position;

            RaycastHit hit;

            if (distance <= shootRange)
            {
                if (Physics.Raycast(transform.position, dir, out hit, shootRange, layer))
                {
                    if (hit.transform.tag == "Player")
                    {
                        action = Enemy.shooting;
                    }
                    else
                    {
                        action = Enemy.walking;
                    }
                }
            }
            else if (Physics.Raycast(transform.position, dir, out hit, searchRadius, layer))
            {
                if (hit.transform.tag != "Player")
                {
                    action = Enemy.walking;
                }
            }
        }

        if (action == Enemy.shooting)
        {
            if(!setTriggerAttack)
            {
                setTriggerAttack = true;
                anim.SetTrigger("Attack");
            }
           
            anim.SetBool("Running", false);
            if (distance > shootRange)
            {
                action = Enemy.running;
                return;
            }

            Vector3 dir = player.position - transform.position;
            RaycastHit hit;

            if (isHit)
            {
                action = Enemy.reposition;
            }
            else
            {
                if (Physics.Raycast(transform.position, dir, out hit, shootRange, layer))
                {
                    if (hit.transform.tag != "Player")
                    {
                        action = Enemy.walking;
                        return;
                    }
                }
                navMeshAgent.SetDestination(transform.position);
                Attack();
                //StartCoroutine(DelayShooting());
            }
        }

        if (action == Enemy.reposition)
        {
            anim.SetBool("Running", true);
            anim.ResetTrigger("Attack");
            setTriggerAttack = false;
            StartCoroutine(Reposition());
        }

        if (action == Enemy.dead)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator DelayShooting()
    {
        yield return new WaitForSeconds(0.847f);
        gunProjectile.GunShoot();
    }
    

    IEnumerator Reposition ()
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        if (!changingPosition && isHit)
        {
           
            gunProjectile.StopShoot();
            yield return new WaitForSeconds(1);

            if (!changingPosition)
            {
                newPosition.y = transform.position.y;
                newPosition.x = transform.position.x + Random.Range(-20, 20);
                newPosition.z = transform.position.z + Random.Range(-20, 20);
                if (navMeshAgent.CalculatePath(newPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    changingPosition = true;
                    navMeshAgent.SetDestination(newPosition);
                }
            }
        }
        else
        {
            if (navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                navMeshAgent.ResetPath();
                newPosition.y = transform.position.y;
                newPosition.x = transform.position.x + Random.Range(-20, 20);
                newPosition.z = transform.position.z + Random.Range(-20, 20);
                if (navMeshAgent.CalculatePath(newPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    changingPosition = true;
                    navMeshAgent.SetDestination(newPosition);
                }
            }
            
            if (transform.position.x == newPosition.x && transform.position.z == newPosition.z)
            {
                float distance = Vector3.Distance(player.position, transform.position);

                action = Enemy.running;
                if (distance > searchRadius)
                {
                    action = Enemy.walking;
                }
                isHit = false;
                changingPosition = false;
            }
        }
    }

    public void HitPlayer()
    {
        isHit = true;
    }

    public override void Attack()
    {
        base.Attack();
        faceTarget();
    }

    private void faceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
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
