using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : Menu
{
    public static OptionsMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one OptionsMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
