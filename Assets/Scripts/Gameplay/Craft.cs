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

    Animator animator;
    int leftBoolID;
    int rightBoolID;
    bool alive = true;
    bool invulnerable = true;
    int invulnerableTimer = 120;
    const int INVULNERABLENGTH = 120;

    public SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Assert(animator);
        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);
    }

    public void SetInvulnerable()
    {
        invulnerable = true;
        invulnerableTimer = INVULNERABLENGTH;
    }

    private void FixedUpdate()
    {
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

        if (InputManager.instance && alive)
        {
            craftData.positionX += InputManager.instance.playerState[0].movement.x * config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y * config.speed;
            newPosition.x = (int)craftData.positionX;
            newPosition.y = (int)craftData.positionY;
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
        GameManager.instance.playerOneCraft = null;

        yield return null;
    }
}

public class CraftData
{
    public float positionX;
    public float positionY;
}
