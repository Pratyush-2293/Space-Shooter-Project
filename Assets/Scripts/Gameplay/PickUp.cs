using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        INVALID,

        Bomb,
        Coin,
        PowerUp,
        BeamUp,
        Options,
        Medal,
        Secret,
        Lives,

        NOOFPICKUPTYPES
    };

    public PickUpConfig config;
    public Vector2 position;
    public Vector2 velocity;

    private void OnEnable()
    {
        position = transform.position;
    }

    private void FixedUpdate()
    {
        // Move
        position.y -= config.fallSpeed;
        transform.position = position;
    }

    public void ProcessPickup(int playerIndex, CraftData craftData)
    {
        switch (config.type)
        {
            case PickUpType.Coin:
                {
                    GameManager.instance.playerCrafts[playerIndex].IncreaseScore(config.coinValue); // temp fix in playerindex
                    break;
                }
            case PickUpType.PowerUp:
                {
                    GameManager.instance.playerCrafts[playerIndex].PowerUp((char)config.powerLevel); // temp fix in player index
                    break;
                }
            default:
                {
                    Debug.LogError("Unprocessed Pickup Type: " + config.type);
                    break;
                }
        };

        Destroy(gameObject);
    }
}
