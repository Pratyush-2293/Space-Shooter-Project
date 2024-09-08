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

    public SoundFX pickupSound = null;

    private void OnEnable()
    {
        position = transform.position;
        velocity.x = Random.Range(-4, 4);
        velocity.y = Random.Range(-4,4);
    }

    private void FixedUpdate()
    {
        // Move
        position += velocity;
        velocity /= 1.3f;
        position.y -= config.fallSpeed;
        if(GameManager.instance && GameManager.instance.progressWindow)
        {
            float posY = GameManager.instance.progressWindow.transform.position.y;
            if (posY < -180)
            {
                GameManager.instance.PickUpFallOffScreen(this);
                Destroy(gameObject);
                return;
            }
        }
        transform.position = position;
    }

    public void ProcessPickup(int playerIndex, CraftData craftData)
    {
        if (pickupSound)
        {
            pickupSound.Play();
        }

        switch (config.type)
        {
            case PickUpType.Coin:
                {
                    ScoreManager.instance.PickupCollected(playerIndex, config.coinValue);
                    break;
                }
            case PickUpType.PowerUp:
                {
                    GameManager.instance.playerCrafts[playerIndex].PowerUp((byte)config.powerLevel, config.surplusValue); 
                    break;
                }
            case PickUpType.Lives:
                {
                    GameManager.instance.playerCrafts[playerIndex].OneUp(config.surplusValue); 
                    break;
                }
            case PickUpType.Secret:
                {
                    ScoreManager.instance.PickupCollected(playerIndex, config.coinValue);
                    break;
                }
            case PickUpType.BeamUp:
                {
                    GameManager.instance.playerCrafts[playerIndex].IncreaseBeamStrength(config.surplusValue);
                    break;
                }
            case PickUpType.Options:
                {
                    GameManager.instance.playerCrafts[playerIndex].AddOption(config.surplusValue); ;
                    break;
                }
            case PickUpType.Bomb:
                {
                    GameManager.instance.playerCrafts[playerIndex].AddBomb(config.bombPower, config.surplusValue);
                    break;
                }
            case PickUpType.Medal:
                {
                    GameManager.instance.playerCrafts[playerIndex].AddMedal(config.medalLevel, config.medalValue);
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
