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

    private void OnEnable()
    {
        UpdateButtons();
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }

    void UpdateButtons()
    {
        // joystick buttons
        for(int b = 0; b < 8; b++)
        {
            p1_buttons[b].GetComponentInChildren<Text>().text = InputManager.instance.GetButtonName(0,b);
            p2_buttons[b].GetComponentInChildren<Text>().text = InputManager.instance.GetButtonName(1, b);
        }

        // key 'buttons'
        for (int k = 0; k < 8; k++)
        {
            p1_keys[k].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyName(0, k);
            p2_keys[k].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyName(1, k);
        }

        // key 'axes'
        for (int a = 0; a < 4; a++)
        {
            p1_keys[8+a].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyAxisName(0, a);
            p2_keys[8+a].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyAxisName(1, a);
        }
    }
}
