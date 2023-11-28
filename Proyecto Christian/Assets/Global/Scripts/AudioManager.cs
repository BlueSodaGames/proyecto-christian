using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] musicSounds, sfxSounds;
    public static AudioManager Instance;
    public AudioSource musicSource, sfxSource;
    public float minTimeBetweenSounds = 0.5f; // Tiempo mínimo entre reproducciones de sonido

    private float lastSoundTime = 0f; // Último momento en que se reprodujo el sonido
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
            // Comprobar si ha pasado suficiente tiempo desde la última reproducción de sonido
            if (Time.time - lastSoundTime >= minTimeBetweenSounds)
            {
                sfxSource.PlayOneShot(s.clip);
                lastSoundTime = Time.time; // Actualizar el último momento en que se reprodujo el sonido
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
