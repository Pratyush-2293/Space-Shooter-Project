using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public int health = 10;
    public float radiusOrWidth = 10;
    public float height = 10;
    public bool box = false;

    private int layerMask = 0;

    private Vector2 halfExtent;

    public bool damagedByBullets = true;
    public bool damagedByBeams = true;
    public bool damagedByBombs = true;

    public bool spawnCyclicPickup = false;
    public PickUp[] spawnSpecificPickup;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("EnemyBullets");

        halfExtent = new Vector3(radiusOrWidth / 2, height / 2, 1);
    }

    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider[] hits = new Collider[maxColliders];
        int noOfHits = 0;
        if (box)
        {
            noOfHits = Physics.OverlapBoxNonAlloc(transform.position, halfExtent, hits, transform.rotation, layerMask);
        }
        else
        {
            noOfHits = Physics.OverlapSphereNonAlloc(transform.position, radiusOrWidth, hits, layerMask);
        }

        if (noOfHits > 0)
        {
            for(int h = 0; h < noOfHits; h++)
            {
                if (damagedByBullets)
                {
                    Bullet b = hits[h].GetComponent<Bullet>();
                    if (b != null)
                    {
                        TakeDamage(1, b.playerIndex);
                        GameManager.instance.bulletManager.DeActivateBullet(b.index);
                    }
                }
                if (damagedByBombs)
                {
                    Bomb bomb = hits[h].GetComponent<Bomb>();
                    if (bomb != null)
                    {
                        TakeDamage(bomb.power, bomb.playerIndex);
                    }
                }
            }
        }
    }

    public void TakeDamage(int ammount, byte fromPlayer)
    {
        health -= ammount;
        if (health <= 0)
        {
            if (fromPlayer < 2)
            {
                GameManager.instance.playerDatas[fromPlayer].chain++;
                GameManager.instance.playerDatas[fromPlayer].chainTimer = PlayerData.MAXCHAINTIMER;
            }

            Vector2 pos = transform.position;
            if (spawnCyclicPickup)
            {
                PickUp spawn = GameManager.instance.GetNextDrop();
                PickUp p = Instantiate(spawn, pos, Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.instance.transform);
                }
            }

            foreach(PickUp pickup in spawnSpecificPickup)
            {
                PickUp p = Instantiate(pickup, pos, Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.instance.transform);
                }
                else
                {
                    Debug.LogError("Failed to spawn pickup.");
                }
            }

            Destroy(gameObject);
        }
    }
}
