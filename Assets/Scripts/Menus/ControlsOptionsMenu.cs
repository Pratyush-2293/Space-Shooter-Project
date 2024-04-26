using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one ControlsOptionsMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
