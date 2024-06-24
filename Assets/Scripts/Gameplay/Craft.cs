using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    CraftData craftData = new CraftData();
    Vector3 newPosition = new Vector3();
    public CraftConfiguration config;
    public int playerIndex;

    public GameObject AftFlame1;
    public GameObject AftFlame2;
    public GameObject LeftFlame;
    public GameObject RightFlame;
    public GameObject FrontFlame1;
    public GameObject FrontFlame2;

    private void FixedUpdate()
    {
        if (InputManager.instance)
        {
            craftData.positionX += InputManager.instance.playerState[0].movement.x * config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y * config.speed;
            newPosition.x = (int)craftData.positionX;
            newPosition.y = (int)craftData.positionY;
            gameObject.transform.position = newPosition;
        }
    }
}

public class CraftData
{
    public float positionX;
    public float positionY;
}
