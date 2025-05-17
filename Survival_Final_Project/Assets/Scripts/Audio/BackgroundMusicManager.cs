using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on BackgroundMusicManager!");
                return;
            }

            if (audioSource.clip == null)
            {
                Debug.LogError("No audio clip assigned to AudioSource!");
                return;
            }

            // Проверка громкости
            if (audioSource.volume == 0)
            {
                Debug.LogWarning("Volume is set to 0!");
                audioSource.volume = 1.0f;
            }

            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("Playing background music: " + audioSource.clip.name);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
