using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public CraftData craftData = new CraftData();
    Vector3 newPosition = new Vector3();
    public CraftConfiguration config;
    public int playerIndex;

    public GameObject AftFlame1;
    public GameObject AftFlame2;
    public GameObject LeftFlame;
    public GameObject RightFlame;
    public GameObject FrontFlame1;
    public GameObject FrontFlame2;

    Animator animator;
    int leftBoolID;
    int rightBoolID;
    bool alive = true;
    bool invulnerable = true;
    int invulnerableTimer = 120;
    const int INVULNERABLENGTH = 120;
    public static int MAXIMUMBEAMCHARGE = 64;

    const byte MAXLIVES = 5;
    const int MAXSMALLBOMBS = 8;
    const int MAXLARGEBOMBS = 5;

    public SpriteRenderer spriteRenderer = null;
    public BulletSpawner[] bulletSpawner = new BulletSpawner[5];
    public Option[] options = new Option[4];
    public GameObject[] optionMarkersL1 = new GameObject[4];
    public GameObject[] optionMarkersL2 = new GameObject[4];
    public GameObject[] optionMarkersL3 = new GameObject[4];
    public GameObject[] optionMarkersL4 = new GameObject[4];

    public Beam beam = null;
    public GameObject bombPrefab = null;

    int layerMask = 0;
    int pickUpLayer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Assert(animator);
        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);

        layerMask = ~LayerMask.GetMask("PlayerBullets") & ~LayerMask.GetMask("PlayerBombs") & ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("GroundEnemy");

        pickUpLayer = LayerMask.NameToLayer("PickUp");
    }

    public void SetInvulnerable()
    {
        invulnerable = true;
        invulnerableTimer = INVULNERABLENGTH;
    }

    private void FixedUpdate()
    {
        if(InputManager.instance && alive)
        {
            //Chain Drop
            if (GameManager.instance.playerDatas[playerIndex].chainTimer > 0)
            {
                GameManager.instance.playerDatas[playerIndex].chainTimer--;
                if (GameManager.instance.playerDatas[playerIndex].chainTimer == 0)
                {
                    GameManager.instance.playerDatas[playerIndex].chain = 0;
                    ScoreManager.instance.UpdateChainMultiplier(playerIndex);
                }
            }

            //Invulnerable Flashing
            if (invulnerable)
            {
                if (invulnerableTimer % 12 < 6)
                {
                    spriteRenderer.material.SetColor("_Overbright", Color.black);
                }
                else
                {
                    spriteRenderer.material.SetColor("_Overbright", Color.white);
                }

                invulnerableTimer--;

                if (invulnerableTimer <= 0)
                {
                    invulnerable = false;
                    spriteRenderer.material.SetColor("_Overbright", Color.black);
                }
            }

            //Hit Detection
            int maxColliders = 10;
            Collider2D[] hits = new Collider2D[maxColliders];

            //Bullet hits
            Vector2 halfSize = new Vector2(3f, 4f); //Acts as hitbox
            int noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, halfSize, 0, hits, layerMask);
            if (noOfHits > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (hit)
                    {
                        if (hit.gameObject.layer != pickUpLayer)
                        {
                            Hit();
                        }
                    }
                }
            }

            //Pickups & Bullet Grazing
            halfSize = new Vector2(15f, 21f); //Acts as hitbox
            noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, halfSize, 0, hits, layerMask);
            if (noOfHits > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (hit)
                    {
                        if (hit.gameObject.layer == pickUpLayer)
                        {
                            PickUp(hit.GetComponent<PickUp>());
                        }
                        else if (craftData.beamCharge < MAXIMUMBEAMCHARGE) //Bullet graze
                        {
                            craftData.beamCharge++;
                            craftData.beamTimer++;
                        }
                    }
                }
            }
        }

        //Movement
        if (InputManager.instance && alive)
        {
            craftData.positionX += InputManager.instance.playerState[0].movement.x * config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y * config.speed;
            if (craftData.positionX < -145) { craftData.positionX = -145; } // Limiting to playfield
            if (craftData.positionX > 145) { craftData.positionX = 145; }
            if (craftData.positionY < -180) { craftData.positionY = -180; }
            if (craftData.positionY > 180) { craftData.positionY = 180; }
            newPosition.x = (int)craftData.positionX;
            if (!GameManager.instance.progressWindow)
            {
                GameManager.instance.progressWindow = GameObject.FindObjectOfType<LevelProgress>();
            }
            if (GameManager.instance.progressWindow)
            {
                newPosition.y = (int)craftData.positionY + GameManager.instance.progressWindow.transform.position.y;
            }
            else
            {
                newPosition.y = (int)craftData.positionY;
            }
            gameObject.transform.position = newPosition;

            if (InputManager.instance.playerState[0].up)
            {
                AftFlame1.SetActive(true);
                AftFlame2.SetActive(true);
            }
            else
            {
                AftFlame1.SetActive(false);
                AftFlame2.SetActive(false);
            }

            if (InputManager.instance.playerState[0].down)
            {
                FrontFlame1.SetActive(true);
                FrontFlame2.SetActive(true);
            }
            else
            {
                FrontFlame1.SetActive(false);
                FrontFlame2.SetActive(false);
            }

            if (InputManager.instance.playerState[0].left)
            {
                RightFlame.SetActive(true);
                animator.SetBool(leftBoolID, true);
            }
            else
            {
                RightFlame.SetActive(false);
                animator.SetBool(leftBoolID, false);
            }

            if (InputManager.instance.playerState[0].right)
            {
                LeftFlame.SetActive(true);
                animator.SetBool(rightBoolID, true);
            }
            else
            {
                LeftFlame.SetActive(false);
                animator.SetBool(rightBoolID, false);
            }

            //Shooting Bullets
            if (InputManager.instance.playerState[0].shoot)
            {
                ShotConfiguration shotConfig = config.shotLevel[craftData.shotPower];

                //Shot
                for(int s = 0; s < 5; s++)
                {
                    bulletSpawner[s].Shoot(shotConfig.spawnerSizes[s]);
                }

                //Options
                for(int o = 0; o < craftData.numberOfEnabledOptions; o++)
                {
                    if(options[o])
                    {
                        options[o].Shoot();
                    }
                }
            }

            //Options Button
            if (!InputManager.instance.playerPrevState[0].options && InputManager.instance.playerState[0].options)
            {
                craftData.optionsLayout++;
                if (craftData.optionsLayout > 3)
                {
                    craftData.optionsLayout = 0;
                }

                SetOptionsLayout(craftData.optionsLayout);
            }

            //Beam
            if (InputManager.instance.playerState[0].beam)
            {
                beam.Fire();
            }

            //Bomb
            if(!InputManager.instance.playerPrevState[0].bomb && InputManager.instance.playerState[0].bomb)
            {
                FireBomb();
            }
        }
    }

    public void PickUp(PickUp pickUp)
    {
        if (pickUp)
        {
            pickUp.ProcessPickup(playerIndex, craftData);
        }
    }

    public void Hit()
    {
        if (!invulnerable)
        {
            Explode();
        }
    }

    public void Explode()
    {
        alive = false;
        StartCoroutine(Exploding());
    }

    IEnumerator Exploding()
    {
        Color col = Color.white;
        for(float redness = 0; redness<=1; redness += 0.3f)
        {
            col.g = 1 - redness;
            col.b = 1 - redness;
            spriteRenderer.color = col;
            yield return new WaitForSeconds(0.1f);
        }

        EffectSystem.instance.CraftExplosion(transform.position);
        Destroy(gameObject);
        GameManager.instance.playerCrafts[playerIndex] = null;

        yield return null;
    }

    public void AddOption(int surplusValue)
    {
        if (craftData.numberOfEnabledOptions < 4)
        {
            options[craftData.numberOfEnabledOptions].gameObject.SetActive(true);
            craftData.numberOfEnabledOptions++;
        }
        else
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
        }
    }

    public void SetOptionsLayout(int layoutIndex)
    {
        Debug.Assert(layoutIndex < 4);

        for(int o = 0; o < 4; o++)
        {
            switch (layoutIndex)
            {
                case 0:
                    options[o].gameObject.transform.position = optionMarkersL1[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL1[o].transform.rotation;
                    break;
                case 1:
                    options[o].gameObject.transform.position = optionMarkersL2[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL2[o].transform.rotation;
                    break;
                case 2:
                    options[o].gameObject.transform.position = optionMarkersL3[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL3[o].transform.rotation;
                    break;
                case 3:
                    options[o].gameObject.transform.position = optionMarkersL4[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL4[o].transform.rotation;
                    break;
            }
        }
    }

    public void IncreaseBeamStrength(int surplusValue)
    {
        if (craftData.beamPower<5)
        {
            craftData.beamPower++;
            UpdateBeam();
        }
        else
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
        }
    }

    void UpdateBeam()
    {
        beam.beamWidth = (craftData.beamPower + 2) * 8f;
    }

    void FireBomb()
    {
        if (craftData.smallBombs > 0)
        {
            craftData.smallBombs--;
            Vector3 pos = transform.position;
            pos.y += 100;
            Bomb bomb = Instantiate(bombPrefab, pos, Quaternion.identity).GetComponent<Bomb>();
            if (bomb)
            {
                bomb.playerIndex = (byte)playerIndex;
            }
        }
    }

    public void PowerUp(byte powerLevel, int surplusValue)
    {
        craftData.shotPower += powerLevel;
        if (craftData.shotPower > 8)
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            craftData.shotPower = 8;
        }
    }

    public void IncreaseScore(int value)
    {
        GameManager.instance.playerDatas[playerIndex].score += value;
        GameManager.instance.playerDatas[playerIndex].stageScore += value;
    }

    public void OneUp(int surplusValue)
    {
        GameManager.instance.playerDatas[playerIndex].lives++;
        if (GameManager.instance.playerDatas[playerIndex].lives > MAXLIVES)
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            GameManager.instance.playerDatas[playerIndex].lives = MAXLIVES;
        }
    }

    public void AddBomb(int power, int surplusValue)
    {
        if (power == 1)
        {
            craftData.smallBombs++;
            if (craftData.smallBombs > MAXSMALLBOMBS)
            {
                craftData.smallBombs = MAXSMALLBOMBS;
                ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            }
        }
        else if (power == 2)
        {
            craftData.largeBombs++;
            if (craftData.largeBombs > MAXLARGEBOMBS)
            {
                craftData.largeBombs = MAXLARGEBOMBS;
                ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            }
        }
        else
        {
            Debug.LogError("Invalid Bomb Power Pickup");
        }
    }

    public void AddMedal(int level, int value)
    {
        ScoreManager.instance.MedalCollected(playerIndex, value);
        IncreaseScore(value);
    }
}

[Serializable]
public class CraftData
{
    public float positionX;
    public float positionY;

    public byte shotPower;
    public byte numberOfEnabledOptions;
    public byte optionsLayout;

    public bool beamFiring;
    public byte beamPower;    // Damage and Width
    public byte beamCharge;   // Picked by charge
    public byte beamTimer;    // Current Charge Level - How Much Beam is Left

    public byte smallBombs;
    public byte largeBombs;
}
