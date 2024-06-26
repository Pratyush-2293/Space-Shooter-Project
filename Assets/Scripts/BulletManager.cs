using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class BulletManager : MonoBehaviour
{
    public Bullet[] bulletPrefabs;

    public enum BulletType
    {
        Bullet1_Size1,
        Bullet1_Size2,
        Bullet1_Size3,
        Bullet2_Size1,
        Bullet2_Size2,
        Bullet3_Size1,
        Bullet3_Size2,
        Bullet3_Size3,
        Bullet3_Size4,
        Bullet4_Size1,
        Bullet4_Size2,
        Bullet4_Size3,
        Bullet4_Size4,
        Bullet5_Size1,
        Bullet5_Size2,
        Bullet5_Size3,
        Bullet5_Size4,
        Bullet6_Size1,
        Bullet6_Size2,
        Bullet6_Size3,
        Bullet6_Size4,
        Bullet7_Size1,
        Bullet7_Size2,
        Bullet7_Size3,
        Bullet7_Size4,
        Bullet8_Size1,
        Bullet8_Size2,
        Bullet8_Size3,
        Bullet8_Size4,
        Bullet9_Size1,
        Bullet9_Size2,
        Bullet9_Size3,
        Bullet9_Size4,
        Bullet10_Size1,
        Bullet10_Size2,
        Bullet10_Size3,
        Bullet10_Size4,
        Bullet11_Size1,
        Bullet11_Size2,
        Bullet11_Size3,
        Bullet11_Size4,
        Bullet12_Size1,
        Bullet12_Size2,
        Bullet12_Size3,
        Bullet12_Size4,
        Bullet12_Size5,
        Bullet12_Size6,
        Bullet12_Size7,
        Bullet12_Size8,
        Bullet12_Size9,
        Bullet12_Size10,
        Bullet12_Size11,
        Bullet12_Size12,
        Bullet12_Size13,
        Bullet12_Size14,
        Bullet12_Size15,
        Bullet12_Size16,
        Bullet13_Size1,
        Bullet13_Size2,
        Bullet13_Size3,
        Bullet13_Size4,
        Bullet14_Size1,
        Bullet14_Size2,
        Bullet14_Size3,
        Bullet14_Size4,
        Bullet15_Size1,
        Bullet15_Size2,
        Bullet15_Size3,
        Bullet15_Size4,
        Bullet16_Size1,
        Bullet16_Size2,
        Bullet16_Size3,
        Bullet16_Size4,
        Bullet17_Size1,
        Bullet17_Size2,
        Bullet17_Size3,
        Bullet18_Size1,
        Bullet18_Size2,
        Bullet18_Size3,
        Bullet19_Size1,
        Bullet19_Size2,
        Bullet19_Size3,
        Bullet19_Size4,
        MAX_TYPES
    }

    const int MAX_BULLET_PER_TYPE = 500;
    const int MAX_BULLET_COUNT = MAX_BULLET_PER_TYPE * (int)BulletType.MAX_TYPES;
    private Bullet[] bullets = new Bullet[MAX_BULLET_COUNT];
    private NativeArray<BulletData> bulletData;
    private TransformAccessArray bulletTransforms;
    ProcessBulletJob jobProcessor;

    void Start()
    {
        bulletData = new NativeArray<BulletData>(MAX_BULLET_COUNT, Allocator.Persistent);
        bulletTransforms = new TransformAccessArray(MAX_BULLET_COUNT);

        int index = 0;
        for(int bulletType = (int)BulletType.Bullet1_Size1; bulletType < (int)BulletType.MAX_TYPES; bulletType++)
        {
            for(int b=0; b < MAX_BULLET_PER_TYPE; b++)
            {
                Bullet newBullet = Instantiate(bulletPrefabs[bulletType]).GetComponent<Bullet>();
                newBullet.gameObject.SetActive(false);
                newBullet.transform.SetParent(transform);
                bullets[index] = newBullet;
                bulletTransforms.Add(bullets[index].transform);
                index++;
            }
        }

        jobProcessor = new ProcessBulletJob { bullets = bulletData };
    }

    private void OnDestroy()
    {
        bulletData.Dispose();
        bulletTransforms.Dispose();
    }

    private int NextFreeBulletIndex(BulletType type)
    {
        int startIndex = (int)type * MAX_BULLET_PER_TYPE;
        for(int b=0; b < MAX_BULLET_PER_TYPE; b++)
        {
            if (!bulletData[startIndex + b].active)
            {
                return startIndex + b;
            }
        }
        return -1;
    }

    public Bullet SpawnBullet(BulletType type, float x, float y, float dX, float dY, float angle)
    {
        int bulletIndex = NextFreeBulletIndex(type);
        if (bulletIndex > -1)
        {
            Bullet result = bullets[bulletIndex];
            result.gameObject.SetActive(true);
            bulletData[bulletIndex] = new BulletData(x, y, dX, dY, angle, (int)type, true);
            bullets[bulletIndex].gameObject.transform.position = new Vector3(x, y, 0);
            return result;
        }
        return null;
    }

    private void FixedUpdate()
    {
        ProcessBullets();
        for(int b=0; b < MAX_BULLET_COUNT; b++)
        {
            if (!bulletData[b].active)
            {
                bullets[b].gameObject.SetActive(false);
            }
        }
    }

    void ProcessBullets()
    {
        JobHandle handler = jobProcessor.Schedule(bulletTransforms);
        handler.Complete();
    }

    public struct ProcessBulletJob : IJobParallelForTransform
    {
        public NativeArray<BulletData> bullets;
        public void Execute(int index, TransformAccess transform)
        {
            bool active = bullets[index].active;
            if (!active)
            {
                return;
            }

            float dX = bullets[index].dX;
            float dY = bullets[index].dY;
            float x = bullets[index].positionX;
            float y = bullets[index].positionY;
            float angle = bullets[index].angle;
            int type = bullets[index].type;

            // Movement Update
            x = x + dX;
            y = y + dY;

            // Check for out of bounds
            if (x < -320)
            {
                active = false;
            }
            if (x > 320)
            {
                active = false;
            }
            if (y < -180)
            {
                active = false;
            }
            if (y > 180)
            {
                active = false;
            }

            bullets[index] = new BulletData(x, y, dX, dY, angle, type, active);

            // Updating Transforms
            if (active)
            {
                Vector3 newPosition = new Vector3(x, y, 0);
                transform.position = newPosition;

                //Facing Rotation
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(dX, dY, 0));
            }
        }
    }
}
