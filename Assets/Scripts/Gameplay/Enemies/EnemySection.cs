using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySection : MonoBehaviour
{
    public List<EnemyState> states = new List<EnemyState>();

    public void EnableSection(string name)
    {
        foreach(EnemyState state in states)
        {
            if(state.stateName == name)
            {
                state.Enable();
            }
        }
    }

    public void DisableSection(string name)
    {
        foreach(EnemyState state in states)
        {
            if(state.stateName == name)
            {
                state.Disable();
            }
        }
    }
}
