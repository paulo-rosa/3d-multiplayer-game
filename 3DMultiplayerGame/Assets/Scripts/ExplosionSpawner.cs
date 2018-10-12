using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosionSpawner : NetworkBehaviour
{
    public GameObject explosionPrefab;

    public void Explode(Vector3 position)
    {
        var explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(explosion);
        StartCoroutine(DestroyExplosion(explosion));
    }

    IEnumerator DestroyExplosion(GameObject explosion)
    {
        yield return new WaitForSeconds(1f);

        Destroy(explosion);
    }
}