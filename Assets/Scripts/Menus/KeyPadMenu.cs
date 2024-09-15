using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyPadMenu : Menu
{
    public static KeyPadMenu instance = null;

    public Text nameText = null;
    public int playerIndex = 0;

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

    public void OnEnterButton()
    {
        ScoreManager.instance.AddScore(GameManager.instance.playerDatas[playerIndex].score, (int)GameManager.instance.gameSession.hardness, nameText.text);
        ScoreManager.instance.SaveScores();

        TurnOff(false);
        SceneManager.LoadScene("MainMenuScene");
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
