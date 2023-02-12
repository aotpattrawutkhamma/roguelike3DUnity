using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillars : MonoBehaviour
{
    public int numberPrefab;
    public EnemyAI prefabsEnemy;
    float dis;
    public int countEnemy;
    [SerializeField]
    float timerNextSpawn;
    [SerializeField]
    float timerNextSpawnMax;
    float minX = 0, maxX = 10;
    float minZ = 0, maxZ = 10;

    float radiusX;
    float radiusZ;

    public float heightY;
    RaycastHit hit;
    public LayerMask layerMask;
    //int countprefabs = 4;

    


    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(transform.position, MainGame.instance.playerCharacter.transform.position);
        SpawnEnemies();

        ChecknumberPrefabs();

    }
    public void SpawnEnemies()
    {
        if (dis < 50f)
        {
            timerNextSpawn -= Time.deltaTime;
            if (timerNextSpawn <= 0)
            {
                for (int i = 0; i < countEnemy; i++)
                {
                    randomRadius();
                    Vector3 ontop = new Vector3(transform.position.x + radiusX, transform.position.y + heightY, transform.position.z + radiusZ);
                    Vector3 randomPoint = new Vector3(transform.position.x + radiusX, transform.position.y, transform.position.z + radiusZ);
                    Vector3 onDown = randomPoint - ontop;
                    //onDown = new Vector3(0, -1, 0);
                    if (Physics.Raycast(ontop, onDown, out hit, 100f, layerMask))
                    {
                        if (hit.transform.tag == "Floor")
                        {
                            Debug.Log("spawn");
                            EnemyAI enespawner = Instantiate(prefabsEnemy, new Vector3(transform.position.x + radiusX, 0, transform.position.z + radiusZ), transform.rotation);
                        }

                    }

                }

                timerNextSpawn = timerNextSpawnMax;

            }

        }
    }


    public void die()
    {
        for (int i = 0; i < SpawnPillars.pillarDestroyed.Length; i++)
        {
            if (SpawnPillars.pillarDestroyed[i] == 0)
            {
                SpawnPillars.pillarDestroyed[i] = numberPrefab;
                break;
            }

        }
        Destroy(gameObject);
        ChecknumberPrefabs();
    }
    public void randomRadius()
    {
        radiusX = Random.Range(minX, maxX);
        radiusZ = Random.Range(minZ, maxZ);

    }

    bool pillarsIsAttack = false;
    //int countDestroy = 0;
    int destroyPillars = 0;
    bool hasHard = false;
    public void ChecknumberPrefabs()
    {

        if (numberPrefab == 1 && destroyPillars == 0)
        {
            pillarsIsAttack = true;
            Debug.Log("ObjectDestroy");
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
        /* if(countDestroy == 1)
         {
             SpawnPillars.HardMode();
         }*/


    }




    //SpawnPillars.HardMode();



}

