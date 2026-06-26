using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Penerapan Singleton untuk AudioManager
    public static AudioManager Instance { get; private set; }

    [Header("BGM Components (Backsound)")]
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    [Header("Audio Components (SFX)")]
    public AudioSource sfxSource;
    public AudioClip chompClip; // Suara waka-waka makan pellet
    public AudioClip gameOverClip; // Suara saat nabrak hantu

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

    private void Start()
    {
        // Mainkan BGM otomatis saat game mulai
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true; // Biar lagunya ngulang terus
            bgmSource.volume = 0.2f; // Atur volume BGM di sini biar ga menimpa SFX
            bgmSource.Play();
        }
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
            sfxSource.PlayOneShot(chompClip, 1.0f);
        }
    }

    // Fungsi untuk dipanggil saat Pac-Man mati
    public void PlayGameOverSFX()
    {
        if (gameOverClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(gameOverClip, 1.0f);
        }
    }
}