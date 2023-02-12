using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Config")]
    public int enemyCountMin;
    public int enemyCountMax;
    public int enemyDelayMin;
    public int enemyDelayMax;
   



    [Header("Area Spawner X-axis")]
    public float minX = 0;
    public float maxX = 10;
    [Header("Area Spawner Y-axis")]
    public float minZ = 0;
    public float maxZ = 10;
    [Header("Enemy Spawner Config End")]
    [Header("Enemy Spawner Config End")]
    [Header("Enemy Spawner Config End")]


    public float height;


    public BaseCharacter[] enemyPrefabs;

    public int WaveCount;
    float dis;
    public bool playerIsGrouded = false;
    float radiusX;
    float radiusZ;
    RaycastHit hit;
    public LayerMask layerMask;
    public bool isInit = false;

    // Start is called before the first frame update
    
    public float enemyDelayCurrent;

    void Start()
    {
        WaveCount = 0;
        enemyDelayCurrent = enemyDelayMax;
        
        //Invoke("SpawnEnemyPoint", delayPerWave);
        MainGame.instance.enemySpawner = this;
    }

    public void Initialize()
    {
        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PillarSpawner.gameEnded)
        {
            if (isInit)
            {
                enemyDelayCurrent -= Time.deltaTime;
                if (enemyDelayCurrent <= 0)
                {
                    SpawnEnemiesRandom();
                }

            }
        }
       

        
       
        
    }

   
    public void SpawnEnemiesRandom()
    {
        int rngTimer = Random.Range(enemyDelayMin, enemyDelayMax + 1);
        enemyDelayCurrent = rngTimer;
        int rngNumber = Random.Range(enemyCountMin, enemyCountMax + 1);
        Debug.Log(rngNumber + "count");
        for (int i = 0; i < rngNumber; i++)
        {
            int rngX = Random.Range(3, 21);
            int rngZ = Random.Range(3, 21);
            Vector3 posTop = new Vector3(MainGame.instance.playerCharacter.transform.position.x + rngX, MainGame.instance.transform.position.y + 200f, MainGame.instance.playerCharacter.transform.position.z + rngZ);
            if(Physics.Raycast(posTop, Vector3.down, out hit,Mathf.Infinity , layerMask))
            {
                if(hit.transform.tag == "Floor")
                {
                 
                    int rngEnemy = Random.Range(0, enemyPrefabs.Length);
                    Instantiate(enemyPrefabs[rngEnemy].gameObject, hit.point, Quaternion.identity);
                }
                else
                {
                    enemyDelayCurrent = 0.1f;
                }
            }
        }
    }
    /*public void spawnEnemies(enemyWave wave)
    {
        if (playerIsGrouded == true)
        {
            wave.delayNextWave -= Time.deltaTime;
            
            if (wave.delayNextWave <= 0f)
            {

                int randomEnemy = Random.Range((int)wave.enemyMin, (int)wave.enemyMax + 1);
                for (int i = 0; i < randomEnemy; i++)
                {
                    RandomRadius();

                    Vector3 onTop = new Vector3(MainGame.instance.playerCharacter.transform.position.x + radiusX, MainGame.instance.transform.position.y + height, MainGame.instance.playerCharacter.transform.position.z + radiusZ);
                    Vector3 random = new Vector3(MainGame.instance.playerCharacter.transform.position.x + radiusX, MainGame.instance.transform.position.y, MainGame.instance.playerCharacter.transform.position.z + radiusZ);
                    Vector3 down = random - onTop;
                    if (Physics.Raycast(onTop, down, out hit, layerMask))
                    {

                        if (hit.transform.tag == "Floor")
                        {
                            int rngEnemy = Random.Range(0, enemyPrefabs.Length);
                            Instantiate(enemyPrefabs[rngEnemy], hit.point, hit.transform.rotation);
                        }
                    }

                }
                WaveCount++;
                wave.delayNextWave = wave.delayNextWaveMax;
            }
            

        }
    }
    */
    public void RandomRadius()
    {

        radiusX = Random.Range(minX, maxX);
        radiusZ = Random.Range(minZ, maxZ);
    }

   

}






