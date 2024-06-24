using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool twoPlayer = false;
    public GameObject[] craftPrefabs;
    Craft playerOneCraft = null;
    void Start()
    {
        if(instance != null)
        {
            Debug.LogError("Trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Game Manager created successfully.");
    }

    public void SpawnPlayer(int playerIndex, int craftType)
    {
        Debug.Assert(craftType < craftPrefabs.Length);
        Debug.Log("Spawning Player " + playerIndex);
        playerOneCraft = Instantiate(craftPrefabs[craftType]).GetComponent<Craft>();
        playerOneCraft.playerIndex = playerIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerOneCraft)
            {
                SpawnPlayer(1, 0);
            }
        }
    }
}
