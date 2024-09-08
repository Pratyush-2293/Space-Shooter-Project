using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public BulletManager.BulletType bulletType = BulletManager.BulletType.Bullet1_Size1;
    public GameObject muzzleFlash = null;
    public BulletSequence sequence;

    public int rate = 1;
    public int speed = 10;
    private int timer = 0;

    public float startAngle = 0;
    public float endAngle = 0;
    public int radialNumber = 1;

    public float dAngle = 0; // change in angle

    public bool autoFireActive = false;
    private bool firing = false;
    private int frame = 0;

    public bool fireAtPlayer = false;
    public bool fireAtTarget = false;
    public GameObject target = null;

    public bool homing = false;

    public SoundFX shootSounds = null;

    //public bool isPlayer = false;
    public byte playerIndex = 2;  // values > 2 are enemy

    public void Shoot(int size)
    {
        if (size < 0)
        {
            return;
        }

        if (playerIndex>1)  // Preventing enemy firing from behind
        {
            float y = transform.position.y;
            if(GameManager.instance && GameManager.instance.progressWindow)
            {
                y -= GameManager.instance.progressWindow.data.positionY;
            }
            if(y<-100 || y > 180)
            {
                return;
            }
        }

        Vector2 primaryDirection = transform.up;

        if (fireAtPlayer || fireAtTarget)
        {
            Vector2 targetPosition = Vector2.zero;
            if (fireAtPlayer)
            {
                if (GameManager.instance.playerCrafts[0])
                {
                    targetPosition = GameManager.instance.playerCrafts[0].transform.position;
                }
            }
            else if(fireAtTarget && target != null)
            {
                targetPosition = target.transform.position;
            }

            primaryDirection = targetPosition - (Vector2)transform.position;
            primaryDirection.Normalize();
        }

        if (firing || timer == 0)
        {
            float angle = startAngle;
            for(int a = 0; a < radialNumber; a++)
            {
                Quaternion myRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 velocity = myRotation * primaryDirection * speed;
                BulletManager.BulletType bulletToShoot = bulletType + size;
                GameManager.instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, angle, dAngle, homing, playerIndex);

                angle = angle + ((endAngle - startAngle) / (radialNumber - 1));
            }
           
            if (muzzleFlash)
            {
                muzzleFlash.SetActive(true);
            }
            if (shootSounds)
            {
                shootSounds.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        timer++;
        if (timer >= rate)
        {
            timer = 0;
            if(muzzleFlash && !InputManager.instance.playerState[0].shoot)
            {
                muzzleFlash.SetActive(false);
            }
            if (autoFireActive)
            {
                firing = true;
                frame = 0;
            }
        }

        if (firing)
        {
            if (sequence.ShouldFire(frame))
            {
                Shoot(1);
            }

            frame++;

            if (frame > sequence.totalFrames)
            {
                firing = false;
            }
        }
    }

    public void Activate()
    {
        autoFireActive = true;
        timer = 0;
        frame = 0;
        firing = true;
    }

    public void DeActivate()
    {
        autoFireActive = false;
        //timer = 0;
        //frame = 0;
        //firing = false;
    }
}

[Serializable]
public class BulletSequence
{
    public List<int> emmitFrames = new List<int>();
    public int totalFrames;

    public bool ShouldFire(int currentFrame)
    {
        foreach(int frame in emmitFrames)
        {
            if (frame == currentFrame)
            {
                return true;
            }
        }
        return false;
    }
}
