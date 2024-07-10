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

    public bool autoFireActive = false;
    private bool firing = false;
    private int frame = 0;

    public bool fireAtPlayer = false;
    public bool fireAtTarget = false;
    public GameObject target = null;

    public void Shoot(int size)
    {
        if (size < 0)
        {
            return;
        }

        if (firing || timer == 0)
        {
            float angle = startAngle;
            for(int a = 0; a < radialNumber; a++)
            {
                Quaternion myRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 velocity = myRotation * transform.up * speed;
                BulletManager.BulletType bulletToShoot = bulletType + size;
                GameManager.instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, 0);

                angle = angle + ((endAngle - startAngle) / (radialNumber - 1));
            }
           
            if (muzzleFlash)
            {
                muzzleFlash.SetActive(true);
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
