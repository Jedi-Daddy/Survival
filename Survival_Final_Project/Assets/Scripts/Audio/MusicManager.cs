using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton для музыки
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MusicManager initialized.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate MusicManager destroyed.");
            return;
        }

        // Получаем или создаем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("AudioSource component was missing, created automatically.");
        }

        // Проверяем наличие клипа
        if (audioSource.clip == null)
        {
            Debug.LogError("No audio clip assigned to AudioSource.");
        }
    }

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.volume = 0.5f;
            audioSource.Play();
            Debug.Log("Music started.");
        }
        else
        {
            Debug.LogError("Cannot play music. AudioSource or clip is missing.");
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Music stopped.");
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            Debug.Log("Volume set to: " + volume);
        }
    }

    public void ToggleMute()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
            Debug.Log("Music mute: " + audioSource.mute);
        }
    }
}
