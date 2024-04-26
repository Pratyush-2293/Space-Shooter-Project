using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
