using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptionsMenu : Menu
{
    public static GraphicsOptionsMenu instance = null;

    public Toggle fullScreenToggle = null;
    bool fullScreenToApply = true;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GraphicsOptionsMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (fullScreenToggle)
        {
            fullScreenToggle.isOn = ScreenManager.instance.fullScreen;
        }

        fullScreenToApply = ScreenManager.instance.fullScreen;
    }

    public void OnApplyButton()
    {
        ScreenManager.instance.fullScreen = fullScreenToApply;
        Screen.fullScreen = fullScreenToApply;

        if (fullScreenToApply)
        {
            Debug.Log("Going Fullscreen");
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            Debug.Log("Going Windowed");
            PlayerPrefs.SetInt("FullScreen", 0);
        }

        PlayerPrefs.Save();
    }

    public void OnFullScreenToggle()
    {
        fullScreenToApply = !fullScreenToApply;
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}
