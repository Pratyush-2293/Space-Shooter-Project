using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem instance = null;
    public GameObject craftExplosionPrefab = null;
    public ParticleSystem craftParticlesPrefab = null;
    public ParticleSystem craftDebrisParticlesPrefab = null;

    public ParticleSystem hitParticleSystem = null;

    public GameObject largeExplosion = null;
    public GameObject smallExplosion = null;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one EffectSystem");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void CraftExplosion(Vector3 position)
    {
        Instantiate(craftExplosionPrefab, position, Quaternion.identity);
        Instantiate(craftParticlesPrefab, position, Quaternion.identity);
        Instantiate(craftDebrisParticlesPrefab, position, Quaternion.identity);
    }

    public void SpawnSparks(Vector3 position)
    {
        Quaternion angle = Quaternion.Euler(0, 0, 45);
        Instantiate(hitParticleSystem, position, angle);
    }

    public void SpawnLargeExplosion(Vector3 position)
    {
        Instantiate(largeExplosion, position, Quaternion.identity);
    }

    public void SpawnSmallExplosion(Vector3 position)
    {
        Instantiate(smallExplosion, position, Quaternion.identity);
    }
}
