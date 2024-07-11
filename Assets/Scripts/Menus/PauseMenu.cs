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
}
