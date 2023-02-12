using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProtector : BaseCharacter
{
    public bool hasTakenThunderClap = false;
    public int listLocation;
    public List<EnemyAI> enemyList;

    private bool changingPosition = false;
    public Pillar owner;
    NavMeshAgent navMeshAgent;

    [SerializeField] Essence essencePrefab;

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
        owner.protectorCount--;
        Destroy(this.gameObject);
    }

    public override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Initiate(Pillar pillar)
    {
        owner = pillar;
    }

    public override void Update()
    {
        StartCoroutine(Reposition());
    }

    IEnumerator Reposition()
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        if (!changingPosition)
        {
            yield return new WaitForSeconds(1);

            if (!changingPosition)
            {
                newPosition.y = transform.position.y;
                newPosition.x = owner.transform.position.x + Random.Range(-30, 30);
                newPosition.z = owner.transform.position.z + Random.Range(-30, 30);
                if (navMeshAgent.CalculatePath(newPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {        
                    float distance = Vector3.Distance(owner.transform.position, newPosition);
                 
                    if (distance <= 50)
                    {
                       
                        changingPosition = true;
                        navMeshAgent.SetDestination(newPosition);
                    }
                }
            }
        }
        else
        {
            if (navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                navMeshAgent.ResetPath();
                newPosition.y = transform.position.y;
                newPosition.x = owner.transform.position.x + Random.Range(-30, 30);
                newPosition.z = owner.transform.position.z + Random.Range(-30, 30);
                if (navMeshAgent.CalculatePath(newPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    float distance = Vector3.Distance(owner.transform.position, newPosition);
                    Debug.Log(distance);
                    if (distance <= 50)
                    {
                        Debug.Log("Lower than 50" + distance);
                        changingPosition = true;
                        navMeshAgent.SetDestination(newPosition);
                    }
                }
            }

            float distanceX = transform.position.x - newPosition.x;
            float distanceY = transform.position.z - newPosition.z;

            if (Mathf.Abs(distanceX) <= 0.1f && Mathf.Abs(distanceY) <= 0.1f)
            {
                changingPosition = false;
            }
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
            Die();
        }
    }
}
