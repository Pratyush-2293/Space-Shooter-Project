using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one CraftSelectMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnPlayButton()
    {
        GameManager.instance.StartGame();
    }

    public void OnCraftA_P1Button()
    {

    }

    public void OnCraftB_P1Button()
    {

    }

    public void OnCraftC_P1Button()
    {

    }

    public void OnCraftX_P1Button()
    {

    }

    public void OnCraftZ_P1Button()
    {

    }

    public void OnCraftA_P2Button()
    {

    }

    public void OnCraftB_P2Button()
    {

    }

    public void OnCraftC_P2Button()
    {

    }

    public void OnCraftX_P2Button()
    {

    }

    public void OnCraftZ_P2Button()
    {

    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}
