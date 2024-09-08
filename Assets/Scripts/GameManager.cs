using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerCrafts[playerIndex] = Instantiate(craftPrefabs[craftType]).GetComponent<Craft>();
        playerCrafts[playerIndex].playerIndex = playerIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerCrafts[0])
            {
                SpawnPlayer(0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerCrafts[0] && playerCrafts[0].craftData.shotPower < CraftConfiguration.MAX_SHOT_POWER)
            {
                playerCrafts[0].craftData.shotPower++;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern =  GameObject.FindObjectOfType<EnemyPattern>();
            testPattern.Spawn();
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

    public void StartGame()
    {
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
}
