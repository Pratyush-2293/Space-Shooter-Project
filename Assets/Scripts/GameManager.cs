using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool twoPlayer = false;
    public GameObject[] craftPrefabs;
    //public Craft playerOneCraft = null;
    public Craft[] playerCrafts = new Craft[2];
    public BulletManager bulletManager = null;
    public LevelProgress progressWindow = null;
    public Session gameSession = new Session();
    public PlayerData[] playerDatas;
    public PickUp[] cyclicDrops = new PickUp[15];
    public PickUp[] medals = new PickUp[10];
    private int currentDropIndex = 0;
    private int currentMedalIndex = 0;
    public enum GameState { INVALID, InMenus, Playing, Paused};
    public GameState gameState = GameState.INVALID;

    public PickUp powerUp = null;
    public PickUp option = null;
    public PickUp beamUp = null;

    void Start()
    {
        if(instance != null)
        {
            Debug.LogError("Trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

        playerDatas = new PlayerData[2];
        playerDatas[0] = new PlayerData();
        playerDatas[1] = new PlayerData();

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Game Manager created successfully.");

        bulletManager = GetComponent<BulletManager>();

        Application.targetFrameRate = 60;
    }

    public void SpawnPlayer(int playerIndex, int craftType)
    {
        Debug.Assert(craftType < craftPrefabs.Length);
        Debug.Log("Spawning Player " + playerIndex);
        if (progressWindow == null)
        {
            progressWindow = GameObject.FindObjectOfType<LevelProgress>();
        }
        Vector3 pos = progressWindow.transform.position;
        int whichPrefab = playerIndex; // TODO: needs to take into consideration the craftType
        playerCrafts[playerIndex] = Instantiate(craftPrefabs[whichPrefab], pos, Quaternion.identity).GetComponent<Craft>();
        playerCrafts[playerIndex].playerIndex = playerIndex;

        if (GameManager.instance.twoPlayer)
        {
            if (playerIndex == 0)
            {
                gameSession.craftDatas[0].positionX = -50;
            }
            else
            {
                gameSession.craftDatas[0].positionX = 50;
            }
        }
    }

    public void SpawnPlayers()
    {
        SpawnPlayer(0, 0); //todo Craft Type
        if (twoPlayer)
        {
            SpawnPlayer(1, 0);
        }
    }

    public void DelayedRespawn(int playerIndex)
    {
        StartCoroutine(RespawnCoroutine(playerIndex));
    }

    IEnumerator RespawnCoroutine(int playerIndex)
    {
        yield return new WaitForSeconds(1.5f);
        SpawnPlayer(playerIndex, 0); //todo get craft type.
        yield return null;
    }

    public void ResetState(int playerIndex)
    {
        CraftData craftData = gameSession.craftDatas[playerIndex];
        craftData.positionX = 0;
        craftData.positionY = 0;
        craftData.shotPower = 0;
        craftData.numberOfEnabledOptions = 0;
        craftData.optionsLayout = 0;
        craftData.beamFiring = false;
        craftData.beamCharge = 0;
        craftData.beamPower = 0;
        craftData.beamTimer = 0;
        craftData.smallBombs = 3;
        craftData.largeBombs = 0;
    }

    public void RestoreState(int playerIndex)
    {
        int number = gameSession.craftDatas[playerIndex].numberOfEnabledOptions;
        gameSession.craftDatas[playerIndex].numberOfEnabledOptions = 0;
        gameSession.craftDatas[playerIndex].positionX = 0;
        gameSession.craftDatas[playerIndex].positionY = 0;

        for(int o = 0; o < number; o++)
        {
            playerCrafts[playerIndex].AddOption(0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerCrafts[0])
            {
                SpawnPlayer(0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            DebugManager.instance.ToggleHUD();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) // Music Tracks
        {
            AudioManager.instance.PlayMusic(AudioManager.Tracks.Level01, true, 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AudioManager.instance.PlayMusic(AudioManager.Tracks.Boss, true, 2);
        }
    }

    public void TogglePause()
    {
        if (gameState == GameState.Playing) //pause the game
        {
            gameState = GameState.Paused;
            AudioManager.instance.PauseMusic();
            PauseMenu.instance.TurnOn(null);
            if (DebugManager.instance.displaying)
            {
                DebugManager.instance.ToggleHUD();
            }
            Time.timeScale = 0;
        }
        else //unpause
        {
            gameState = GameState.Playing;
            AudioManager.instance.ResumeMusic();
            PauseMenu.instance.TurnOff(false);
            Time.timeScale = 1;
        }
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        ResetState(0);
        ResetState(1);
        playerDatas[0].ResetData();
        playerDatas[1].ResetData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage01");
    }

    public void PickUpFallOffScreen(PickUp pickup)
    {
        if(pickup.config.type == PickUp.PickUpType.Medal)
        {
            currentMedalIndex = 0;
        }
    }

    public PickUp GetNextDrop()
    {
        PickUp result = cyclicDrops[currentDropIndex];

        if(result.config.type == PickUp.PickUpType.Medal)
        {
            result = medals[currentMedalIndex];
            currentMedalIndex++;
            if (currentMedalIndex > 9)
            {
                currentMedalIndex = 0;
            }
        }

        currentDropIndex++;
        if (currentDropIndex > 14)
        {
            currentDropIndex = 0;
        }

        return result;
    }

    public PickUp SpawnPickup(PickUp pickUpPrefab, Vector2 pos)
    {
        PickUp p = Instantiate(pickUpPrefab, pos, Quaternion.identity);
        if (p)
        {
            p.transform.SetParent(GameManager.instance.transform);
        }

        return p;
    }

    public void ResumeGameFromLoad()
    {
        gameState = GameState.Playing;
        switch (gameSession.stage)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage01");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage02");
                break;
        }
    }

    public void NextStage()
    {
        HUD.instance.FadeOutScreen();

        if(gameSession.stage == 1)
        {
            gameSession.stage = 2;
            SceneManager.LoadScene("Stage02");
        }
        else if(gameSession.stage == 2)
        {
            WellDoneMenu.instance.TurnOn(null);
        }

        HUD.instance.FadeInScreen();
    }
}
