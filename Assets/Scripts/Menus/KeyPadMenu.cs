using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyPadMenu : Menu
{
    public static KeyPadMenu instance = null;

    public Text nameText = null;
    public Text enterText = null;
    public int playerIndex = 0;
    public bool bothPlayers = false;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one KeyPadMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public override void TurnOn(Menu previous)
    {
        base.TurnOn(previous);

        enterText.text = "Enter name player " + (playerIndex+1);
    }

    public void OnEnterButton()
    {
        ScoreManager.instance.AddScore(GameManager.instance.playerDatas[playerIndex].score, (int)GameManager.instance.gameSession.hardness, nameText.text);
        ScoreManager.instance.SaveScores();

        if(bothPlayers && playerIndex == 0)
        {
            playerIndex = 1;
            enterText.text = "Enter name player " + (playerIndex+1);
            nameText.text = "";
        }
        else
        {
            TurnOff(false);
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void OnKeyPress(int key)
    {
        nameText.text += (char)key;
    }

    public void OnClearButton()
    {
        nameText.text = "";
    }

    public void OnDeleteButton()
    {
        if (nameText.text.Length > 0)
        {
            nameText.text = nameText.text.Substring(0, nameText.text.Length - 1);
        }
    }
}
