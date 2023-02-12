using System.Collections.Generic;
using UnityEngine;


public class PillarSpawner : MonoBehaviour
{
    public static bool gameEnded = false;
    public static int[] pillarDestroyed = new int[4];
    public GameObject pillarPrefab;
    public  int pillarAmounts;
    public  List<Transform> pillarLocations = new List<Transform>();
    public GameObject[] pillarArray = new GameObject[4];
    public bool isHardModeTriggered = false;

    void Start()
    {
        SpawnPillar();
        for (int i = 0; i < pillarDestroyed.Length; i++)
        {
            pillarDestroyed[i] = 0;
        }
        MainGame.instance.pillarSpawner = this;
       
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void SpawnPillar()
    {
        
        for (int countPillars = 0; countPillars < pillarAmounts; countPillars++)
        {
            pillarArray[countPillars] = Instantiate(pillarPrefab);

            pillarPrefab.GetComponent<PillarUi>().NumberPrefabs = (countPillars + 1);
            pillarPrefab.GetComponent<Pillar>().numberPrefab = (countPillars + 1);

        }
        


        for (int i = 0; i < pillarAmounts; i++)
        {
           
            Transform selectedLocation = pillarLocations[Random.Range(0, pillarLocations.Count)];
            pillarArray[i].transform.position = selectedLocation.position;
            pillarLocations.Remove(selectedLocation);

        }

        
        
    }
    public static void HardMode()
    {
        MainGame.instance.gameDifficulty = MainGame.instance.gameDifficulty * 1.25f;
    }
   
    public void CheckDestroyedPillarOrder()
    {
        bool isInOrder = true;
        for (int i = 0; i < pillarDestroyed.Length; i++)
        {
            if(pillarDestroyed[i] == i + 1 || pillarDestroyed[i] == 0)
            {

            }
            else
            {
                isInOrder = false;
            }

        }

        if(!isInOrder)
        {
            if(!isHardModeTriggered)
            {
                isHardModeTriggered = true;
                HardMode();
            }
        }
        gameEnded = HasAllPillarBeenDestroyed();
    }
    public bool HasAllPillarBeenDestroyed()
    {
        bool a = true;
        for (int i = 0; i < pillarDestroyed.Length; i++)
        {
            if(pillarDestroyed[i] == 0)
            {
                a = false;
            }
        }
        return a;
    }

}





