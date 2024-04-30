using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu instance = null;
    public Button[] p1_buttons = new Button[8];
    public Button[] p2_buttons = new Button[8];
    public Button[] p1_keys = new Button[12];
    public Button[] p2_keys = new Button[12];

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

    public void OnBackButton()
    {
        TurnOff(true);
    }
}
