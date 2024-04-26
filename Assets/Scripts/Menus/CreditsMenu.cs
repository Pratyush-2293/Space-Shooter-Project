using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : Menu
{
    public static CreditsMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one CreditsMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
