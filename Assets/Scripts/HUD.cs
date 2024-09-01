using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public AnimatedNumber[] playerScore = new AnimatedNumber[2];
    public AnimatedNumber topScore;
    public GameObject player2Start;

    public PlayerHUD[] playerHUDs = new PlayerHUD[2];

    private void FixedUpdate()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (!GameManager.instance)
        {
            return;
        }

        //Score
        if (playerScore[0]) //Player 1 Score
        {
            int p1Score = GameManager.instance.playerDatas[0].score;
            playerScore[0].UpdateNumber(p1Score);
        }

        UpdateLives(0);
        UpdateBombs(0);
        UpdatePower(0);

        if (GameManager.instance.twoPlayer)
        {
            if (player2Start)
            {
                player2Start.SetActive(false);

                if (playerScore[1]) // Player 2 Score
                {
                    int p1Score = GameManager.instance.playerDatas[1].score;
                    playerScore[1].UpdateNumber(p1Score);
                }

                UpdateLives(1);
                UpdateBombs(1);
                UpdatePower(1);
            }
            else
            {
                if (player2Start)
                {
                    player2Start.SetActive(true);
                }
            }
        }
    }

    private void UpdateLives(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);

        PlayerData data = GameManager.instance.playerDatas[playerIndex];
        PlayerHUD hud = playerHUDs[playerIndex];

        int lives = data.lives;

        for(int i = 0; i < 5; i++)
        {
            if (lives > i)
            {
                hud.lives[i].SetActive(true);
            }
            else
            {
                hud.lives[i].SetActive(false);
            }
            
        }
    }

    private void UpdateBombs(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex]) //Craft does not exist; clearing bomb data.
        {
            for(int i = 0; i < 5; i++)
            {
                hud.bigBombs[i].SetActive(false);
            }
            for (int i = 0; i < 8; i++)
            {
                hud.smallBombs[i].SetActive(false);
            }
            return;
        }

        CraftData data = GameManager.instance.playerCrafts[playerIndex].craftData;

        int largeBombs = data.largeBombs;
        int smallBombs = data.smallBombs;

        for(int i = 0; i < 5; i++) // Handling BigBombs
        {
            if (largeBombs > i)
            {
                hud.bigBombs[i].SetActive(true);
            }
            else
            {
                hud.bigBombs[i].SetActive(false);
            }
        }

        for (int i = 0; i < 8; i++) // Handling SmallBombs
        {
            if (smallBombs > i)
            {
                hud.smallBombs[i].SetActive(true);
            }
            else
            {
                hud.smallBombs[i].SetActive(false);
            }
        }
    }

    private void UpdatePower(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            for(int i = 0; i < 8; i++)
            {
                hud.powerMarks[i].SetActive(false);
            }
            return;
        }

        CraftData data = GameManager.instance.playerCrafts[playerIndex].craftData;

        int power = data.shotPower;
        for(int i = 0; i < 8; i++)
        {
            if (power > i)
            {
                hud.powerMarks[i].SetActive(true);
            }
            else
            {
                hud.powerMarks[i].SetActive(false);
            }
        }
    }

    [Serializable]
    public class PlayerHUD
    {
        public GameObject[] lives = new GameObject[5];
        public GameObject[] bigBombs = new GameObject[5];
        public GameObject[] smallBombs = new GameObject[8];

        public AnimatedNumber chainScore;
        public Image chainGradient;

        public GameObject[] powerMarks = new GameObject[8];
        public GameObject[] beamMarks = new GameObject[5];
        public Image beamGradient;

        public Image progressGradient;

        public AnimatedNumber stageScore;

        public GameObject[] buttons = new GameObject[4];
        public GameObject up;
        public GameObject down;
        public GameObject left;
        public GameObject right;
        public GameObject joystick;

        public Image speedStat;
        public Image powerStat;
        public Image beamStat;
        public Image optionStat;
        public Image bombStat;
    }
}
