using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPillars : MonoBehaviour
{
    public static int[] pillarDestroyed = new int[4];
  
    public GameObject prefab;
    public int countPrefabs;
    //public Transform[] spawnSpot;
    public  List<Transform> spot = new List<Transform>();
    public  Dictionary<int, Transform> spotPillars = new Dictionary<int, Transform>();
     public GameObject[] objArr = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        spawnerPillars();
        for (int i = 0; i < pillarDestroyed.Length; i++)
        {
            pillarDestroyed[i] = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void spawnerPillars()
    {
        
        for (int countPillars = 0; countPillars < countPrefabs; countPillars++)
        {
            objArr[countPillars] = Instantiate(prefab);

            prefab.GetComponent<UiPillars>().NumberPrefabs = (countPillars + 1);
            prefab.GetComponent<Pillars>().numberPrefab = (countPillars + 1);

        }
        for (int i = 1; i < spot.Count; i++)
        {


            spotPillars.Add(i, spot[i - 1]);


        }



        for (int i = 0; i < countPrefabs; i++)
        {
            Transform select = spot[Random.Range(0, spot.Count)];
            objArr[i].transform.position = select.position;
            spot.Remove(select);

        }

        
        
    }
    public static void HardMode()
    {

    }
   
    
}





