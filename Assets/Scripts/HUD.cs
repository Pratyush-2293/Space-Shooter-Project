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
        UpdateBeam(0);
        UpdateControls(0);
        UpdateStats(0);
        UpdateStageScore(0);
        UpdateProgress(0);
        UpdateChain(0);

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
                UpdateBeam(1);
                UpdateControls(1);
                UpdateStats(1);
                UpdateStageScore(1);
                UpdateProgress(1);
                UpdateChain(1);
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

    private void UpdateBeam(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            for(int i = 0; i < 5; i++)
            {
                hud.beamMarks[i].SetActive(false);
                hud.beamGradient.fillAmount = 0;
            }
            return;
        }

        CraftData data = GameManager.instance.playerCrafts[playerIndex].craftData;
        int beam = data.beamPower;

        for(int i = 0; i < 5; i++)
        {
            if (beam > i)
            {
                hud.beamMarks[i].SetActive(true);
            }
            else
            {
                hud.beamMarks[i].SetActive(false);
            }
        }

        hud.beamGradient.fillAmount = (float)data.beamTimer / (float)Craft.MAXIMUMBEAMCHARGE;
    }

    private void UpdateControls(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            for(int i = 0; i < 4; i++)
            {
                hud.buttons[i].SetActive(false);
                hud.left.SetActive(false);
                hud.right.SetActive(false);
                hud.up.SetActive(false);
                hud.down.SetActive(false);
            }
            return;
        }

        InputState state = InputManager.instance.playerState[playerIndex];

        if (state.shoot)
        {
            hud.buttons[0].SetActive(true);
        }
        else
        {
            hud.buttons[0].SetActive(false);
        }

        if (state.beam)
        {
            hud.buttons[1].SetActive(true);
        }
        else
        {
            hud.buttons[1].SetActive(false);
        }

        if (state.bomb)
        {
            hud.buttons[2].SetActive(true);
        }
        else
        {
            hud.buttons[2].SetActive(false);
        }

        if (state.options)
        {
            hud.buttons[3].SetActive(true);
        }
        else
        {
            hud.buttons[3].SetActive(false);
        }

        if (state.left)
        {
            hud.left.SetActive(true);
        }
        else
        {
            hud.left.SetActive(false);
        }

        if (state.right)
        {
            hud.right.SetActive(true);
        }
        else
        {
            hud.right.SetActive(false);
        }

        if (state.up)
        {
            hud.up.SetActive(true);
        }
        else
        {
            hud.up.SetActive(false);
        }

        if (state.down)
        {
            hud.down.SetActive(true);
        }
        else
        {
            hud.down.SetActive(false);
        }

        hud.joystick.SetActive(true);
        hud.joystick.transform.localPosition = new Vector2(-296, -147) + state.movement * 3;
    }

    private void UpdateStats(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            hud.speedStat.fillAmount = 0;
            hud.powerStat.fillAmount = 0;
            hud.beamStat.fillAmount = 0;
            hud.optionStat.fillAmount = 0;
            hud.bombStat.fillAmount = 0;

            return;
        }

        CraftConfiguration config = GameManager.instance.playerCrafts[playerIndex].config;

        hud.speedStat.fillAmount = config.speed / (float)CraftConfiguration.MAX_SPEED;
        hud.powerStat.fillAmount = config.bulletStrength / (float)CraftConfiguration.MAX_SHOT_POWER;
        hud.beamStat.fillAmount = config.beamPower / (float)CraftConfiguration.MAX_BEAM_POWER;
        hud.optionStat.fillAmount = config.optionPower / (float)CraftConfiguration.MAX_OPTION_POWER;
        hud.bombStat.fillAmount = config.bombPower / (float)CraftConfiguration.MAX_BOMB_POWER;
    }

    private void UpdateStageScore(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            hud.stageScore.UpdateNumber(0);
            return;
        }

        hud.stageScore.UpdateNumber(GameManager.instance.playerDatas[playerIndex].stageScore);
    }

    private void UpdateProgress(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance || !GameManager.instance.progressWindow)
        {
            hud.progressGradient.fillAmount = 1;
            return;
        }

        float progress = GameManager.instance.progressWindow.data.positionY / (float)GameManager.instance.progressWindow.levelSize;

        hud.progressGradient.fillAmount = 1 - progress;
    }

    private void UpdateChain(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHUD hud = playerHUDs[playerIndex];

        if (!GameManager.instance.playerCrafts[playerIndex])
        {
            hud.chainScore.UpdateNumber(0);
            return;
        }

        hud.chainScore.UpdateNumber(GameManager.instance.playerDatas[playerIndex].chain);

        hud.chainGradient.fillAmount = GameManager.instance.playerDatas[playerIndex].chainTimer / (float)PlayerData.MAXCHAINTIMER;
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