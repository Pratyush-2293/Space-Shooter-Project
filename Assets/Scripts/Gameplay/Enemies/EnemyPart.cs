using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    public bool destroyed = false;
    public bool damaged = false;

    public void Destroyed()
    {
        destroyed = true;
        Enemy enemy = transform.root.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.PartDestroyed();
        }
    }

    public void Damaged()
    {
        damaged = true;
    }
}
