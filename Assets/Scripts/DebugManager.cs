using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance = null;

    public bool displaying = false;
    public GameObject ROOT = null;

    public Toggle invincibleToggle = null;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Debug Manager.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ToggleHUD()
    {
        if (!displaying) // turn on
        {
            if (!ROOT)
            {
                Debug.LogError("ROOT gameobject not set!");
            }
            else
            {
                ROOT.SetActive(true);
                displaying = true;
                Time.timeScale = 0; // pause the game.
                Cursor.visible = true;
            }
        }
        else // turn off
        {
            if (!ROOT)
            {
                Debug.LogError("ROOT gameobject not set!");
            }
            else
            {
                ROOT.SetActive(false);
                displaying = false;
                Time.timeScale = 1; // resume the game.
                Cursor.visible = false;
            }
        }
    }

    public void ToggleInvincibility()
    {
        if (invincibleToggle)
        {
            GameManager.instance.gameSession.invincible = invincibleToggle.isOn;
        }
    }
}
