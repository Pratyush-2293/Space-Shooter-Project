using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool twoPlayer = false;
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
}
