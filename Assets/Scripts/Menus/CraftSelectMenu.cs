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
}
