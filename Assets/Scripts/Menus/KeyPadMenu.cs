using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadMenu : Menu
{
    public static KeyPadMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one KeyPadMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
