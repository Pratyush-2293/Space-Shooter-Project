using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : Menu
{
    public static PlayMenu instance = null;

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
}
