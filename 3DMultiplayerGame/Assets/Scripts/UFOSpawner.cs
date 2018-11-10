using UnityEngine;
using UnityEngine.Networking;

public class UFOSpawner : NetworkBehaviour
{
    public GameObject UFOPrefab;

    public override void OnStartServer()
    {
        var spawnPosition = transform.position;
        var spawnRotation = transform.rotation;

        var UFO = Instantiate(UFOPrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(UFO);
    }
}