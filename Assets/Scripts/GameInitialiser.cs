using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Scene displayScene;
    
    void Start()
    {
        if(GameManager.instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
                displayScene = SceneManager.GetSceneByName("DisplayScene");
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
            if (!displayScene.isLoaded)
            {
                SceneManager.LoadScene("DisplayScene", LoadSceneMode.Additive);
            }

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
