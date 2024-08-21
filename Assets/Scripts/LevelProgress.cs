using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public ProgressData data;
    public AnimationCurve speedCurve = new AnimationCurve();
    public int levelSize;
    public GameObject midGroundTileGrid;
    public float midGroundRate = 0.75f;
    private Craft player1Craft;

    private void Start()
    {
        data.positionX = transform.position.x;
        data.positionY = transform.position.y;

        if (GameManager.instance)
        {
            GameManager.instance.progressWindow = this;
        }
    }

    private void Update()
    {
        if (data.progress < levelSize)
        {
            float ratio = (float)data.progress / (float)levelSize;
            float movement = speedCurve.Evaluate(ratio);
            data.progress++;
            
            if (player1Craft == null)
            {
                player1Craft = GameManager.instance.playerCrafts[0];
            }
            if (player1Craft)
            {
                UpdateProgressWindow(player1Craft.craftData.positionX, movement);
            }
        }
    }

    void UpdateProgressWindow(float shipX, float movement)
    {
        data.positionX = shipX / 10f;
        data.positionY += movement;
        transform.position = new Vector3(data.positionX, data.positionY, 0);
        midGroundTileGrid.transform.position = new Vector3(0, midGroundRate * data.positionY, 350);
    }
}

[Serializable]
public class ProgressData
{
    public int progress;
    public float positionX;
    public float positionY;
}
