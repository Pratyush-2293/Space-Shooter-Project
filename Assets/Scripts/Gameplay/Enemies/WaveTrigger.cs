using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public EnemyPattern[] patterns = null;
    public float[] delays = null;

    public int waveBonus = 0;
    public bool spawnCyclicPickup = false;
    public PickUp[] spawnSpecificPickup;

    public int noOfenemies = 0;

    private void Start()
    {
        foreach(EnemyPattern pattern in patterns)
        {
            if (pattern != null)
            {
                noOfenemies++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //SpawnWave();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        int i = 0;
        foreach(EnemyPattern pattern in patterns)
        {
            if (pattern)
            {
                pattern.owningWave = this;
                Session.Hardness hardness = GameManager.instance.gameSession.hardness;
                if (pattern.spawnOnEasy && hardness == Session.Hardness.Easy)
                {
                    pattern.Spawn();
                }
                if (pattern.spawnOnNormal && hardness == Session.Hardness.Normal)
                {
                    pattern.Spawn();
                }
                if (pattern.spawnOnHard && hardness == Session.Hardness.Hard)
                {
                    pattern.Spawn();
                }
                if (pattern.spawnOnInsane && hardness == Session.Hardness.Insane)
                {
                    pattern.Spawn();
                }
            }
            i++;
        }
        yield return null;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach(EnemyPattern pattern in patterns)
        {
            Handles.DrawLine(transform.position, pattern.transform.position);
        }
    }
    #endif

    public void EnemyDestroyed(Vector3 pos, int playerIndex)
    {
        noOfenemies--;
        if (noOfenemies == 0) // None Left!
        {
            ScoreManager.instance.ShootableDestroyed(playerIndex, waveBonus);

            if (spawnCyclicPickup)
            {
                PickUp spawn = GameManager.instance.GetNextDrop();
                PickUp p = Instantiate(spawn, pos, Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.instance.transform);
                }
            }

            foreach(PickUp pickUp in spawnSpecificPickup)
            {
                PickUp p = Instantiate(pickUp, pos, Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.instance.transform);
                }
            }
        }
    }
}
