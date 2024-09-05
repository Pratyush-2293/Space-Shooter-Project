using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public int health = 10;
    public float radiusOrWidth = 10;
    public float height = 10;
    public bool box = false;
    public bool polygon = false;

    public bool remainDestroy = false;
    private bool destroyed = false;
    public int damageHealth = 5;      //at what health the damaged sprite is displayed.

    private Collider2D polyCollider;

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

        if (polygon)
        {
            polyCollider = GetComponent<PolygonCollider2D>();
            Debug.Assert(polyCollider);
        }
        else
        {
            halfExtent = new Vector3(radiusOrWidth / 2, height / 2, 1);
        }
    }

    private void FixedUpdate()
    {
        if (destroyed)
        {
            return;
        }

        int maxColliders = 10;
        Collider2D[] hits = new Collider2D[maxColliders];
        int noOfHits = 0;
        if (box)
        {
            noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, halfExtent, 0, hits, layerMask); //here 0 is representing transform.rotation
        }
        else if (polygon)
        {
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(layerMask);
            contactFilter.useLayerMask = true;
            noOfHits = Physics2D.OverlapCollider(polyCollider, contactFilter, hits);
        }
        else
        {
            noOfHits = Physics2D.OverlapCircleNonAlloc(transform.position, radiusOrWidth, hits, layerMask);
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
        if (destroyed)
        {
            return;
        }

        health -= ammount;

        EnemyPart part = GetComponent<EnemyPart>();
        if (health <= damageHealth)
        {
            part.Damaged(true);
        }
        else
        {
            part.Damaged(false);
        }

        if (health <= 0)
        {
            destroyed = true;
            if (part)
            {
                part.Destroyed();
            }

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

            if (remainDestroy)
            {
                destroyed = true;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
