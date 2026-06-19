using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Lives { get; private set; } = 3;

    private void Awake()
    {
        // Mencegah ada lebih dari 1 GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Biar gak hancur pas pindah scene
    }

    // Subscribe ke event (Jadi Subscriber)
    private void OnEnable()
    {
        GameEvents.OnPelletEaten += AddScore;
    }

    // Unsubscribe biar nggak memory leak / error
    private void OnDisable()
    {
        GameEvents.OnPelletEaten -= AddScore;
    }

    // Otomatis jalan kalau ada yang manggil TriggerPelletEaten
    private void AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"Skor Sekarang: {Score}");
    }

    public void LoseLife()
    {
        Lives--;
        Debug.Log($"Nyawa sisa: {Lives}");
        if (Lives <= 0)
        {
            GameEvents.TriggerGameOver();
        }
    }
}