using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{

    public float hp = 100;
    public float hpMax = 100;
    [Header("Distance untill become hostile")]
    public float aggressionDistance;
    [Header("Spawn Range")]
    [SerializeField] float minX = 1;
    [SerializeField] float maxX = 10;
    [SerializeField] float minZ = 1;
    [SerializeField] float maxZ = 10;
    [Header("Timer between each wave")]
    [SerializeField] float timerNextSpawnMax;
    [Header("Enemy per wave")]
    public int countEnemy;
    [Header("Protector Spawn Rate")]
    public int randomProtectorRate;
    [Header("Time between each spawn")]
    [SerializeField]float timeProtectorSpawnMax = 10;
    [Header("End Config")]
    [Header("End Config")]
    [Header("End Config")]


    public int numberPrefab;
    public BaseCharacter[] enemyPrefab;
    public EnemyProtector prefabEnemyProtector;
    float dis;
    
    [SerializeField]
    float timerNextSpawn;
   
   

    float radiusX;
    float radiusZ;

    public float heightY;
    RaycastHit hit;
    public LayerMask layerMask;
    //int countprefabs = 4;

    bool pillarsIsAttack = false;
    //int countDestroy = 0;
    int destroyPillars = 0;
    bool hasHard = false;

    

    


    public Protector prefabsProtector;
    public int protectorCount = 0;
    public float timeProtectorSpawn = 10;
    
    

    void Start()
    {
        SpawnProtectorGauranteed(1);
        timerNextSpawn = timerNextSpawnMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PillarSpawner.gameEnded)
        {
            dis = Vector3.Distance(transform.position, MainGame.instance.playerCharacter.transform.position);
            SpawnEnemies();

        }


    }
    public void SpawnEnemies()
    {
        if (dis < aggressionDistance)
        {
            SpawnProtector();
            timerNextSpawn -= Time.deltaTime;

            if (timerNextSpawn <= 0)
            {

                for (int i = 0; i < countEnemy; i++)
                {
                    RandomRadius();
                    Vector3 ontop = new Vector3(transform.position.x + radiusX, transform.position.y + heightY, transform.position.z + radiusZ);
                    Vector3 randomPoint = new Vector3(transform.position.x + radiusX, transform.position.y, transform.position.z + radiusZ);
                    Vector3 onDown = randomPoint - ontop;
                    //onDown = new Vector3(0, -1, 0);
                    if (Physics.Raycast(ontop, onDown, out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.transform.tag == "Floor")
                        {
                            int rng = Random.Range(0, enemyPrefab.Length);
                            Instantiate(enemyPrefab[rng], new Vector3(transform.position.x + radiusX, 0, transform.position.z + radiusZ), transform.rotation);
                            
                        }
                        else
                        {
                            i--;
                        }

                    }

                }

                timerNextSpawn = timerNextSpawnMax;

            }

        }
    }
    void SpawnProtectorGauranteed(int number)
    {
        for (int i = 0; i < number; i++)
        {
            RandomRadius();
            Vector3 ontop = new Vector3(transform.position.x + radiusX, transform.position.y + heightY, transform.position.z + radiusZ);
            if (Physics.Raycast(ontop, Vector3.down, out hit,Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag == "Floor")
                {
                    EnemyProtector eneProtector = Instantiate(prefabEnemyProtector, hit.point, transform.rotation);
                    protectorCount++;
                    eneProtector.owner = this;

                }
                else
                {
                    i--;
                }
            }
           
        }
        
    }
    void SpawnProtector()
    {

        timeProtectorSpawn -= Time.deltaTime;
        if (timeProtectorSpawn <= 0)
        {
            int rng = Random.Range(0, 100);
            if (rng < randomProtectorRate)
            {
                RandomRadius();
                Vector3 ontop = new Vector3(transform.position.x + radiusX, transform.position.y + heightY, transform.position.z + radiusZ);
                if (Physics.Raycast(ontop, Vector3.down, out hit,Mathf.Infinity, layerMask))
                {
                    if (hit.transform.tag == "Floor")
                    {
                        int rng2 = Random.Range(0, enemyPrefab.Length);
                        EnemyProtector eneProtector = Instantiate(prefabEnemyProtector, hit.point, transform.rotation);
                        protectorCount++;
                        eneProtector.owner = this;
                        timeProtectorSpawn = timeProtectorSpawnMax;
                    }

                }
                else
                {
                    timeProtectorSpawn = 0.1f;
                }
                
               
            }
            else
            {
                timeProtectorSpawn = timeProtectorSpawnMax;
            }
        }

    }

    public void Die()
    {
        for (int i = 0; i < PillarSpawner.pillarDestroyed.Length; i++)
        {
            if (PillarSpawner.pillarDestroyed[i] == 0)
            {
                PillarSpawner.pillarDestroyed[i] = numberPrefab;
                break;
            }

        }
        MainGame.instance.pillarSpawner.CheckDestroyedPillarOrder();
        MainGame.instance.playerCharacter.PillarDestroyedSound();
        Destroy(gameObject);
     
    }
    public void RandomRadius()
    {
        radiusX = Random.Range(minX, maxX);
        radiusZ = Random.Range(minZ, maxZ);

    }

    /*public void ChecknumberPrefabs()
    {

        if (numberPrefab == 1 && destroyPillars == 0)
        {
            pillarsIsAttack = true;
            
            destroyPillars++;

        }
        else if (numberPrefab == 2 && destroyPillars == 1)
        {
            destroyPillars++;
        }
        else if (numberPrefab == 3 && destroyPillars == 2)
        {
            destroyPillars++;
        }
        else if (numberPrefab == 4 && destroyPillars == 3)
        {

        }
        else
        {
            pillarsIsAttack = false;
        }
        if (pillarsIsAttack == false)
        {

            destroyPillars--;
            //countDestroy++;
            if (hasHard == false)
            {
                hasHard = true;
                //hard mode
            }
        }
         if(countDestroy == 1)
         {
             SpawnPillars.HardMode();
         }


    }*/


    public void TakeDamage(float damage)
    {
        if (protectorCount == 0)
        {
            hp -= damage;
            if(hp <= 0)
            {
                Die();
            }
        }


    }




    //SpawnPillars.HardMode();



}

