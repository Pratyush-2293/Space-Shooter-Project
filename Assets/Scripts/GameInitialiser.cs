using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    public enum GameMode
    {
        INVALID,
        Menus,
        Gameplay
    }
    public GameMode gameMode;
    public GameObject gameManagerPrefab = null;
    private bool menuLoaded = false;
    
    void Start()
    {
        if(GameManager.instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("gameManagerPrefab is not set!");
            }
        }
    }

    private void Update()
    {
        if (!menuLoaded)
        {
            switch (gameMode)
            {
                case GameMode.Menus:
                    MenuManager.instance.SwitchToMainMenus();
                    break;

                case GameMode.Gameplay:
                    MenuManager.instance.SwitchToGameplayMenus();
                    break;
            };

            menuLoaded = true;
        }
    }
}
