using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spline : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> controlPoints;

    [SerializeField]
    public bool isClosed;

    [HideInInspector]
    public List<Vector2> linearPoints = new List<Vector2>();

    public int NoOfSegments()
    {
        return controlPoints.Count / 3;
    }

    public Vector2[] GetSegmentPoints(int s, Vector2 offset)
    {
        return new Vector2[]
        {
            controlPoints[s*3] + offset,
            controlPoints[s*3+1] + offset,
            controlPoints[s*3+2] + offset,
            controlPoints[s*3+3] + offset
        };
    }

    public Spline()
    {
        controlPoints = new List<Vector2>
        {
            Vector2.zero,
            Vector2.down * 20,
            (Vector2.down + Vector2.right) * 20,
            (Vector2.down + Vector2.right + Vector2.down) * 20
        };

        CalculatePoints(4);
    }

    public void CalculatePoints(float speed)
    {
        linearPoints.Clear();

        for(int s = 0; s < NoOfSegments(); s++)
        {
            Vector2[] segPoints = GetSegmentPoints(s, Vector2.zero);

            int divisions = 50;

            float t = 0;
            while (t <= 1)
            {
                t += 1f / divisions;
                Vector2 pointOnCurve = Bezier.CalculateCubic(segPoints[0], segPoints[1], segPoints[2], segPoints[3], t);

                linearPoints.Add(pointOnCurve);
            }
        }
    }

    public void SetPoint(int index, Vector2 newPosition)
    {
        Vector2 dPos = newPosition - controlPoints[index];
        controlPoints[index] = newPosition;

        if (index % 3 == 0) //anchor point
        {
            if (index + 1 < controlPoints.Count)
            {
                controlPoints[index + 1] += dPos;
            }
            if (index - 1 >= 0)
            {
                controlPoints[index - 1] += dPos;
            }
        }
    }
}
