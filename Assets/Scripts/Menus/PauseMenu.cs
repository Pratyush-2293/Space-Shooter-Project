using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    public static PauseMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one PlayMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnNormalButton()
    {
        TurnOff(false);
        CraftSelectMenu.instance.TurnOn(this);
    }

    public void OnBulletHellButton()
    {
        TurnOff(false);
        CraftSelectMenu.instance.TurnOn(this);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }

    public void OnResumeButton()
    {
        GameManager.instance.TogglePause();
    }

    public void OnLoadButton()
    {
        if (SaveManager.instance.LoadExists(1))
        {
            SaveManager.instance.LoadGame(1);
        }
    }

    public void OnSaveButton()
    {
        SaveManager.instance.SaveGame(1);
    }

    public void OnOptionsButton()
    {

    }

    public void OnMainMenuButton()
    {

    }
}
