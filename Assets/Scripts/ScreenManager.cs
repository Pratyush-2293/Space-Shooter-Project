using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance = null;

    public bool fullScreen = true;
    Resolution currentResolution;
    Resolution[] allResolutions;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one ScreenManager!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        currentResolution = Screen.currentResolution;
        allResolutions = Screen.resolutions;
    }

    public void SetResolution(Resolution res)
    {
        if (fullScreen)
        {
            Screen.SetResolution(res.width, res.height, FullScreenMode.ExclusiveFullScreen, res.refreshRate);
        }
        else
        {
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed, res.refreshRate);
        }

        PlayerPrefs.SetInt("ScreenWidth", res.width);
        PlayerPrefs.SetInt("ScreenHeight", res.height);
        PlayerPrefs.SetInt("ScreenRate", res.refreshRate);

        Cursor.visible = false;
    }

    void RestoreSettings()
    {
        // Restore Resolution
        int width = 1280;
        int height = 720;
        int rate = 60;
        if (PlayerPrefs.HasKey("ScreenWidth"))
        {
            width = PlayerPrefs.GetInt("ScreenWidth");
        }
        if (PlayerPrefs.HasKey("ScreenHeight"))
        {
            height = PlayerPrefs.GetInt("ScreenHeight");
        }
        if (PlayerPrefs.HasKey("ScreenRate"))
        {
            rate = PlayerPrefs.GetInt("ScreenRate");
        }

        Resolution res = FindResolution(width, height, rate);
        SetResolution(res);

        // Restore Fullscreen Settings
        if (!PlayerPrefs.HasKey("FullScreen"))
        {
            int fullScreenInt = PlayerPrefs.GetInt("FullScreen");
            if (fullScreenInt == 0)
            {
                fullScreen = false;
            }
            else if (fullScreenInt == 1)
            {
                fullScreen = true;
            }
            else
            {
                Debug.LogError("FullScreen preference is invalid!");
            }
        }
        Screen.fullScreen = fullScreen;
    }

    Resolution FindResolution(int width, int height, int rate)
    {
        foreach(Resolution res in allResolutions)
        {
            if((res.width == width) && (res.height == height) && (res.refreshRate == rate))
            {
                return res;
            }
        }
        return currentResolution;
    }
}
