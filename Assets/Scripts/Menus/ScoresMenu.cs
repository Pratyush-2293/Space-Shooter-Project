using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresMenu : Menu
{
    public static ScoresMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one ScoresMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
