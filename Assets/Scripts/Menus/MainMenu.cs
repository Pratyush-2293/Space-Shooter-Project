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
        PlayMenu.instance.TurnOn(this);
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
}
