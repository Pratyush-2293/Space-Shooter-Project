using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public LineRenderer lineRenderer = null;
    public float beamWidth = 20;
    public Craft craft = null;
    private int layerMask;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("PlayerBullets") & ~LayerMask.GetMask("Player");
    }

    public void Fire()
    {
        if (!craft.craftData.beamFiring)
        {
            craft.craftData.beamFiring = true;
            craft.craftData.beamTimer = craft.craftData.beamCharge;
            UpdateBeam();
            gameObject.SetActive(true);
        }
        
    }

    private void FixedUpdate()
    {
        UpdateBeam();
    }

    void UpdateBeam()
    {
        craft.craftData.beamTimer--;
        if (craft.craftData.beamTimer==0)
        {
            craft.craftData.beamFiring = false;
            gameObject.SetActive(false);
        }

        int maxColliders = 20;
        Collider[] hits = new Collider[maxColliders];
        float middleY = (craft.transform.position.y + 180) * 0.5f;
        Vector2 halfSize = new Vector2(beamWidth * 0.5f, (180 - craft.transform.position.y) * 0.5f);
        Vector3 center = new Vector3(craft.transform.position.x, middleY, 0);
        int noOfHits = Physics.OverlapBoxNonAlloc(center, halfSize, hits, Quaternion.identity, layerMask);
        float lowest = 180;
        if (noOfHits > 0)
        {
            for(int hit = 0; hit < noOfHits; hit++)
            {
                RaycastHit hitInfo;
                Ray ray = new Ray(craft.transform.position, Vector3.up);
                float height = 180 - craft.transform.position.y;
                if(hits[hit].Raycast(ray, out hitInfo, height))
                {
                    if (hitInfo.point.y < lowest)
                    {
                        lowest = hitInfo.point.y;
                    }
                }
            }
        }

        //Update Visuals
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;

        lineRenderer.SetPosition(0, transform.position);
        Vector3 top = transform.position;
        top.y = lowest;
        lineRenderer.SetPosition(1, top);
    }
}
