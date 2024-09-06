using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private EnemyPattern pattern;
    private EnemySection[] sections;
    public EnemyRule[] rules;

    public bool isBoss = false;
    private int timer;
    public int timeOut = 3600;
    private bool timedOut = false;

    Animator animator = null;
    public string timeOutParameterName = null;

    private void Start()
    {
        sections = gameObject.GetComponentsInChildren<EnemySection>();
        animator = gameObject.GetComponentInChildren<Animator>();
        timer = timeOut;
    }

    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }

    private void FixedUpdate()
    {
        //timeout
        if (isBoss)
        {
            if(timer<=0 && !timedOut)
            {
                timedOut = true;
                if (animator)
                {
                    animator.SetTrigger(timeOutParameterName);
                }
                sections[0].EnableState("TimeOut");
            }
            else
            {
                timer--;
            }
        }

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

        //Update state timers
        foreach(EnemySection section in sections)
        {
            section.UpdateStateTimers();
        }
    }

    public void TimeOutDestruct()
    {
        Destroy(gameObject);
    }

    void OutOfBounds()
    {
        Destroy(gameObject);
    }

    public void EnableState(string name)
    {
        foreach(EnemySection section in sections)
        {
            section.EnableState(name);
        }
    }

    public void DisableState(string name)
    {
        foreach (EnemySection section in sections)
        {
            section.DisableState(name);
        }
    }

    public void PartDestroyed()
    {
        // Go through all rules and check for parts matching ruleset.
        foreach(EnemyRule rule in rules)
        {
            if (!rule.triggered)
            {
                int noOfDestroyedParts = 0;
                foreach(EnemyPart part in rule.partsToCheck)
                {
                    if (part.destroyed)
                    {
                        noOfDestroyedParts++;
                    }
                }
                if (noOfDestroyedParts >= rule.noOfPartsRequired)
                {
                    rule.triggered = true;
                    rule.ruleEvents.Invoke();
                }
            }
        }
    }

    public void Destroyed()
    {
        Destroy(gameObject);
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
