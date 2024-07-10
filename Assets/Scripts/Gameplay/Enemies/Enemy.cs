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

        pattern.Calculate(transform, data.ProgressTimer);
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
