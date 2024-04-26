using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMenu : Menu
{
    public static TitleScreenMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one TitleScreenMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
