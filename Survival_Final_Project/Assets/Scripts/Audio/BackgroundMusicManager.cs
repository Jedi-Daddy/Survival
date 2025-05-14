using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Serializable]
    public class MusicTrack
    {
        public string Name;
        public AudioClip Clip;
    }

    public static BackgroundMusicManager Instance;

    public AudioSource Source;
    public MusicTrack[] Tracks;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("BackgroundMusicManager initialized.");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Проверяем наличие компонента AudioSource
        if (Source == null)
        {
            Source = GetComponent<AudioSource>();
            if (Source == null)
            {
                Debug.LogError("AudioSource component not found on BackgroundMusicManager!");
                return;
            }
        }

        Source.loop = true;
        Source.playOnAwake = false;  // Отключаем автозапуск
    }

    void Start()
    {
        // Попробуем запустить первый трек при старте
        if (Tracks.Length > 0)
        {
            Play(Tracks[0].Name);  // ✅ Запуск первого трека из списка
        }
        else
        {
            Debug.LogError("No tracks available in BackgroundMusicManager.");
        }
    }

    public void Play(string name)
    {
        var track = Tracks.FirstOrDefault(x => x.Name.Equals(name));
        if (track == null)
        {
            Debug.LogError("Music track with name " + name + " not found");
            return;
        }

        if (Source.clip == track.Clip && Source.isPlaying)
        {
            Debug.Log("Music track " + name + " is already playing.");
            return;
        }

        Source.clip = track.Clip;
        if (Source.clip == null)
        {
            Debug.LogError("Assigned clip is null.");
            return;
        }

        Source.Play();
        if (Source.isPlaying)
        {
            Debug.Log("Music is now playing: " + name);
        }
        else
        {
            Debug.LogError("Music failed to start playing: " + name);
        }
    }

    public void Stop()
    {
        if (Source.isPlaying)
        {
            Source.Stop();
            Debug.Log("Background music stopped.");
        }
    }

    public void Pause()
    {
        if (Source.isPlaying)
        {
            Source.Pause();
            Debug.Log("Background music paused.");
        }
    }

    public void Resume()
    {
        if (!Source.isPlaying && Source.clip != null)
        {
            Source.Play();
            Debug.Log("Background music resumed.");
        }
    }

    public void SetVolume(float volume)
    {
        Source.volume = Mathf.Clamp01(volume);
        Debug.Log("Music volume set to: " + volume);
    }

    public void ToggleMute()
    {
        Source.mute = !Source.mute;
        Debug.Log("Music mute: " + Source.mute);
    }

    public void NextTrack()
    {
        if (Tracks.Length == 0) return;

        int currentIndex = Array.FindIndex(Tracks, t => t.Clip == Source.clip);
        int nextIndex = (currentIndex + 1) % Tracks.Length;

        Play(Tracks[nextIndex].Name);
    }
}
