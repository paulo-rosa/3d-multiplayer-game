using UnityEngine;
using UnityEngine.Networking;

public class UFOSpawner : NetworkBehaviour
{
    public GameObject UFOPrefab;

    public override void OnStartServer()
    {       
        var spawnPosition = new Vector3(
            Random.Range(-8.0f, 8.0f),
            0.0f,
            Random.Range(-8.0f, 8.0f));

        var spawnRotation = Quaternion.Euler(
            0.0f,
            Random.Range(0, 180),
            0.0f);

        var UFO = Instantiate(UFOPrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(UFO);
    }
}