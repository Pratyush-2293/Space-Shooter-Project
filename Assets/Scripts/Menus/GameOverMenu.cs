using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : Menu
{
    public static GameOverMenu instance = null;

    public Text scoreReadout = null;
    public Text hiScoreReadout = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GameOverMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnContinueButton()
    {
        if (ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[0].score, (int)GameManager.instance.gameSession.hardness) ||
            ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[1].score, (int)GameManager.instance.gameSession.hardness))
        {
            if (GameManager.instance.twoPlayer)
            {
                if(ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[0].score, (int)GameManager.instance.gameSession.hardness))
                {
                    KeyPadMenu.instance.playerIndex = 0;
                }
                else
                {
                    KeyPadMenu.instance.playerIndex = 1;
                }

                if (ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[0].score, (int)GameManager.instance.gameSession.hardness) ||
                    ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[1].score, (int)GameManager.instance.gameSession.hardness))
                {
                    KeyPadMenu.instance.bothPlayers = true;
                }
                else
                {
                    KeyPadMenu.instance.bothPlayers = false;
                }
            }
            else
            {
                KeyPadMenu.instance.playerIndex = 0;
                KeyPadMenu.instance.bothPlayers = false;
            }

            KeyPadMenu.instance.TurnOn(null);
            TurnOff(false);
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void GameOver()
    {
        TurnOn(null);
        AudioManager.instance.PlayMusic(AudioManager.Tracks.GameOver, true, 0.5f);
        scoreReadout.text = GameManager.instance.playerDatas[0].score.ToString(); //todo score readout for player 2.

        if(ScoreManager.instance.IsHiScore(GameManager.instance.playerDatas[0].score, (int)GameManager.instance.gameSession.hardness))
        {
            hiScoreReadout.gameObject.SetActive(true);
        }
        else
        {
            hiScoreReadout.gameObject.SetActive(false);
        }
    }
}
