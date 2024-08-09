using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private EnemyPattern pattern;
    private EnemySection[] sections;

    private void Start()
    {
        sections = gameObject.GetComponentsInChildren<EnemySection>();
    }

    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }

    private void FixedUpdate()
    {
        data.ProgressTimer++;
        if (pattern)
        {
            pattern.Calculate(transform, data.ProgressTimer);
        }

        //Off-Screen Check
        float y = transform.position.y;
        if(GameManager.instance && GameManager.instance.progressWindow)
        {
            y -= GameManager.instance.progressWindow.data.positionY;
        }
        if (y < -200)
        {
            OutOfBounds();
        }

    }

    void OutOfBounds()
    {
        Destroy(gameObject);
    }

    public void EnableState(string name)
    {
        foreach(EnemySection section in sections)
        {
            section.EnableSection(name);
        }
    }

    public void DisableState(string name)
    {
        foreach (EnemySection section in sections)
        {
            section.DisableSection(name);
        }
    }
}

[Serializable]
public struct EnemyData
{
    public float ProgressTimer;
    public float positionX;
    public float positionY;

    public int patternUID;
}
