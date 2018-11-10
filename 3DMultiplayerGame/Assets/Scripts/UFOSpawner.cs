using UnityEngine;
using UnityEngine.Networking;

public class UFOSpawner : NetworkBehaviour
{
    public GameObject UFOPrefab;
    private bool triggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerEntered)
        {
            triggerEntered = true;
            SpawnUFO();            
        }
    }

    private void SpawnUFO()
    {
        var spawnPosition = transform.position + new Vector3(0, 15f, 0);
        var spawnRotation = transform.rotation;

        var UFO = Instantiate(UFOPrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(UFO);
    }
}