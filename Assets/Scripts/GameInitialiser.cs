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

    public int stageNumber = 0;

    public GameMode gameMode;
    public GameObject gameManagerPrefab = null;
    private bool menuLoaded = false;

    private Scene displayScene;

    public AudioManager.Tracks playMusicTrack = AudioManager.Tracks.None;
    
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
                    GameManager.instance.gameState = GameManager.GameState.InMenus;
                    break;

                case GameMode.Gameplay:
                    MenuManager.instance.SwitchToGameplayMenus();
                    GameManager.instance.gameState = GameManager.GameState.Playing;
                    GameManager.instance.gameSession.stage = stageNumber;
                    break;
            };

            if(playMusicTrack != AudioManager.Tracks.None)
            {
                AudioManager.instance.PlayMusic(playMusicTrack, true, 1);
            }

            if(gameMode == GameMode.Gameplay)
            {
                SaveManager.instance.SaveGame(0); //0 = autosave at beginning of the state
                GameManager.instance.SpawnPlayers();
            }

            menuLoaded = true;
        }
    }
}
