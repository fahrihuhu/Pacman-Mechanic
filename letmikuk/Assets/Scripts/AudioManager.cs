using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Penerapan Singleton untuk AudioManager
    public static AudioManager Instance { get; private set; }

    [Header("Audio Components")]
    public AudioSource sfxSource;
    public AudioClip chompClip; // Suara waka-waka makan pellet

    private void Awake()
    {
        // Mencegah ada double AudioManager saat pindah-pindah scene
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Biar musik/suara gak mati pas ganti level
    }

    // Subscribe ke event makan pellet (Observer Pattern)
    private void OnEnable()
    {
        GameEvents.OnPelletEaten += PlayChompSFX;
    }

    private void OnDisable()
    {
        GameEvents.OnPelletEaten -= PlayChompSFX;
    }

    // Fungsi ini otomatis jalan tiap kali event ke-trigger
    private void PlayChompSFX(int scoreAmount)
    {
        if (chompClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(chompClip);
        }
    }
}