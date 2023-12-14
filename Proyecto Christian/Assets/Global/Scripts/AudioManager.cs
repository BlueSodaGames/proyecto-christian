using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] musicSounds, sfxSounds;
    public static AudioManager Instance;
    public AudioSource musicSource, sfxSource;
    public float minTimeBetweenSounds = 0.5f;

    private float lastSoundTime = 0f;
    public bool title = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Marcar el objeto AudioManager para no ser destruido al cambiar de escena
        //DontDestroyOnLoad(audioManager.gameObject);
    }

    void Start()
    {
        if (title)
        {
            PlayMusic("TitleTheme");
        }
        else
        {
            PlayMusic("Theme");
        }
    }

    public void SetTitleFalse()
    {
        title = false;
        StopMusic("TitleTheme");
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
            
        
    }

    public void StopMusic(string name)
    {

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Stop();
        }


    }

    public void PlaySFX(string name)
    {

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
        Debug.LogWarning("Sound: " + name + "not found.");
        return;

        }
        else
        {
            if (Time.time - lastSoundTime >= minTimeBetweenSounds)
            {
                sfxSource.PlayOneShot(s.clip);
                lastSoundTime = Time.time;
            }

        }

        
    }


    public void StopSfx(string name)
    {
        Sound s = Array.Find(sfxSounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;

        }

        s.source.Stop();

    }
}
