using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : Menu
{
    public static GameOverMenu instance = null;
    public Text scoreReadout = null;

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
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GameOver()
    {
        TurnOn(null);
        AudioManager.instance.PlayMusic(AudioManager.Tracks.GameOver, true, 0.5f);
        scoreReadout.text = GameManager.instance.playerDatas[0].score.ToString(); //todo score readout for player 2.
    }
}
