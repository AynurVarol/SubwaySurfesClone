using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaySpawner : MonoBehaviour
{
    public GameObject[] groundWayPrefabs;
   float nextSpawnPoint = 0;
    public float wayLength;
    public int numberOfWays = 5;

    public List<GameObject> activeWays = new List<GameObject>();
    public Transform playerTransform;



    private void Start()
    { 
        for(int i = 0; i < numberOfWays; i++)
        {
            if (i == 0)
                SpawnWay(0);
            else
            SpawnWay(Random.Range(0, groundWayPrefabs.Length));
        }
    }

    private void Update()
    {
        if (playerTransform.position.z -35  > nextSpawnPoint - (numberOfWays * wayLength))
        {
            SpawnWay(Random.Range(0, groundWayPrefabs.Length));
            DeleteWay();
        }
    }
    public void SpawnWay(int wayIndex)
    {
     GameObject gameobject = Instantiate(groundWayPrefabs[wayIndex],transform.forward * nextSpawnPoint,transform.rotation );
        gameobject.transform.position = new Vector3(transform.position.x, transform.position.y, gameobject.transform.position.z);
        activeWays.Add(gameobject);

        nextSpawnPoint += wayLength;

    }

    private void DeleteWay()
    {
        Destroy(activeWays[0]);
        activeWays.RemoveAt(0);
    }
    
}