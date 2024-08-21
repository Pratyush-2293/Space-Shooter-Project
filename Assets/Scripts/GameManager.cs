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

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].AddOption();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].IncreaseBeamStrength();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern =  GameObject.FindObjectOfType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (bulletManager)
            {
                bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3, 0, 150, Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0, 0, false);
            }
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage01");
    }
}
