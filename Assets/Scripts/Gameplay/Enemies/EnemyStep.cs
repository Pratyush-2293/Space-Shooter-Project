using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStep
{
    public enum MovementType
    {
        INVALID,

        none,      //waiting at a position
        direction,
        spline,
        atTarget,
        homing,
        follow,
        circle,

        NOOFMOVEMENTTYPES
    }

    [SerializeField]
    public MovementType movement;

    [SerializeField]
    public Vector2 direction;

    [SerializeField]
    public Spline spline;

    [SerializeField]
    [Range(1, 20)]
    public float movementSpeed = 4;

    [SerializeField]
    public float framesToWait = 30;

    public List<string> activateStates = new List<string>();
    public List<string> deActivateStates = new List<string>();

    public EnemyStep(MovementType inMovement)
    {
        movement = inMovement;
        direction = Vector2.zero;

        if(inMovement == MovementType.spline)
        {
            spline = new Spline();
        }
    }

    public float TimeToComplete()
    {
        if(movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            return timeToTravel;
        }
        else if(movement == MovementType.none)
        {
            return framesToWait;
        }
        else if(movement == MovementType.spline)
        {
            return spline.Length() / movementSpeed;

        }

        Debug.LogError("TimeToComplete unable to process movement type, returning 1.");
        return 1;
    }

    public Vector2 EndPosition(Vector3 startPosition)
    {
        Vector2 result = startPosition;

        if(movement == MovementType.direction)
        {
            result += direction;
            return result;
        }
        else if(movement == MovementType.none)
        {
            return startPosition;
        }
        else if (movement == MovementType.spline)
        {
            result += (spline.LastPoint() - spline.StartPoint());
            return result;
        }

        Debug.LogError("EndPosition unable to process movement type, returning 1.");
        return result;
    }

    public Vector3 CalculatePosition(Vector2 startPos, float stepTime)
    {
        float normalizedTime = stepTime / TimeToComplete();
        if(normalizedTime < 0)
        {
            normalizedTime = 0;
        }

        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            float ratio = stepTime / timeToTravel;

            Vector2 place = startPos + (direction * ratio);
            return place;
        }
        else if (movement == MovementType.none)
        {
            return startPos;
        }
        else if(movement == MovementType.spline)
        {
            return spline.GetPosition(normalizedTime) + startPos;
        }

        Debug.LogError("CalculatePosition unable to process movement type, returning startPosition.");
        return startPos;
    }

    public void FireDeActivateStates(Enemy enemy)
    {
        foreach(string state in activateStates)
        {
            enemy.EnableState(state);
        }
    }

    public void FireActivateStates(Enemy enemy)
    {
        foreach (string state in deActivateStates)
        {
            enemy.DisableState(state);
        }
    }
}
