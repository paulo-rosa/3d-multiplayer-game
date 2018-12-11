using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsPoints : MonoBehaviour
{
    private static SpawnsPoints _instance;
    public static SpawnsPoints Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpawnsPoints>();
            }
            return _instance;
        }
    }

    private List<Transform> spawnPoints = new List<Transform>();

    // Use this for initialization
    private void Start ()
    {
        FindSpawnPoints();
	}
	
    private void FindSpawnPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var point = transform.GetChild(i);

            spawnPoints.Add(point);
        }
    }
	
    public  Transform GetSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }
}
