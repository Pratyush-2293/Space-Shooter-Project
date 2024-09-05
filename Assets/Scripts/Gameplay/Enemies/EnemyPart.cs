using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPart : MonoBehaviour
{
    public bool destroyed = false;
    public bool damaged = false;

    bool usingDamagedSprite = false;
    public Sprite damagedVersion = null;
    public Sprite destroyedVersion = null;

    public UnityEvent triggerOnDestroyed;

    public void Destroyed()
    {
        if (destroyed)
        {
            return;
        }

        triggerOnDestroyed.Invoke();

        if (destroyedVersion)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sprite = destroyedVersion;
            }
        }

        destroyed = true;
        Enemy enemy = transform.root.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.PartDestroyed();
        }
    }

    public void Damaged(bool switchToDamagedSprite)
    {
        if(destroyed)
        {
            return;
        }

        if(switchToDamagedSprite && !usingDamagedSprite)
        {
            if (damagedVersion)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                {
                    spriteRenderer.sprite = damagedVersion;
                }
            }

            usingDamagedSprite = true;
        }

        damaged = true;
    }
}
