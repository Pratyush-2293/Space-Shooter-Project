using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public EnemyPattern[] patterns = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnWave();
    }

    public void SpawnWave()
    {
        foreach(EnemyPattern pattern in patterns)
        {
            Session.Hardness hardness = GameManager.instance.gameSession.hardness;
            if(pattern.spawnOnEasy && hardness == Session.Hardness.Easy)
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
}
