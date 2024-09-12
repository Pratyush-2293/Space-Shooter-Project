using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour
{
    public UnityEvent eventToTrigger;
    public AudioManager.Tracks musicToTrigger = AudioManager.Tracks.None;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventToTrigger.Invoke();
        if(musicToTrigger != AudioManager.Tracks.None)
        {
            AudioManager.instance.PlayMusic(musicToTrigger, true, 1f);
        }
    }
}
