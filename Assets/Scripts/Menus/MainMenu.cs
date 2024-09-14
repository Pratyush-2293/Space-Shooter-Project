using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public static MainMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MainMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnPlayButton()
    {
        TurnOff(true);
        CraftSelectMenu.instance.TurnOn(this);
        //SceneManager.LoadScene("CraftSelectMenu");
    }

    public void OnPracticeButton()
    {
        TurnOff(true);
        PracticeMenu.instance.TurnOn(this);
    }

    public void OnOptionsButton()
    {
        TurnOff(true);
        OptionsMenu.instance.TurnOn(this);
    }

    public void OnScoresButton()
    {
        TurnOff(true);
        ScoresMenu.instance.TurnOn(this);
    }

    public void OnLoadButton()
    {
        if (SaveManager.instance.LoadExists(1))
        {
            TurnOff(false);
            SaveManager.instance.LoadGame(1);
        }
    }

    public void OnQuitButton()
    {
        TurnOff(true);
        YesNoMenu.instance.TurnOn(this);
    }
}
