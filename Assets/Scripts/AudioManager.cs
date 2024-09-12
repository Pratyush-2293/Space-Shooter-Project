using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource musicSource1 = null;
    public AudioSource musicSource2 = null;
    public AudioSource sfxSource = null;
    public AudioMixer mixer = null;

    public enum Tracks
    {
        Level01,
        Boss,
        GameOver,
        Menu,
        None
    }

    public AudioClip[] musicTracks;

    private int activeMusicSource = 0;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one AudioManager.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        //Restore Preferences
        float volume = PlayerPrefs.GetFloat("MasterVolume");
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        volume = PlayerPrefs.GetFloat("EffectsVolume");
        mixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);

        volume = PlayerPrefs.GetFloat("MusicVolume");
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void PlayMusic(Tracks track, bool fade, float fadeDuration)
    {
        if(activeMusicSource == 0 || activeMusicSource == 2)
        {
            if (!fade)
            {
                mixer.SetFloat("Music1Volume", Mathf.Log10(1)*20); //HI VALUE
                mixer.SetFloat("Music2Volume", Mathf.Log10(0.0001f)); //LO VALUE
            }
            else
            {
                if(activeMusicSource == 0)
                {
                    mixer.SetFloat("Music1Volume", Mathf.Log10(0.0001f));
                    mixer.SetFloat("Music2Volume", Mathf.Log10(0.0001f));
                }
            }

            musicSource1.clip = musicTracks[(int)track];
            musicSource1.Play();
            activeMusicSource = 1;

            if (fade)
            {
                StartCoroutine(Fade(1, fadeDuration, 0, 1));
                if(activeMusicSource == 2 || activeMusicSource == 1)  //Experimental (activeMusicSource == 1)
                {
                    StartCoroutine(Fade(2, fadeDuration, 1, 0));
                }
            }
        }
        else if(activeMusicSource == 1)
        {
            if (!fade)
            {
                mixer.SetFloat("Music1Volume", Mathf.Log10(0.0001f));
                mixer.SetFloat("Music1Volume", Mathf.Log10(1)*20);
                musicSource1.Stop();
            }

            musicSource2.clip = musicTracks[(int)track];
            musicSource2.Play();
            activeMusicSource = 2;

            if (fade)
            {
                StartCoroutine(Fade(2, fadeDuration, 0, 1));
                StartCoroutine(Fade(1, fadeDuration, 1, 0));
            }
        }
    }

    IEnumerator Fade(int sourceIndex, float duration, float startVolume, float targetVolume)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float newVol = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            newVol = Mathf.Clamp(newVol, 0.0001f, 0.999f);

            if(sourceIndex == 1)
            {
                mixer.SetFloat("Music1Volume", Mathf.Log10(newVol) * 20);
            }
            else if(sourceIndex == 2)
            {
                mixer.SetFloat("Music2Volume", Mathf.Log10(newVol) * 20);
            }

            yield return null;
        }

        if(targetVolume <= 0.0001f) //stop
        {
            if(sourceIndex == 1)
            {
                musicSource1.Stop();
            }
            else if (sourceIndex == 2)
            {
                musicSource2.Stop();
            }
        }

        yield return null;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
