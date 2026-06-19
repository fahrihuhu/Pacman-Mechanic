using UnityEngine;

public class GhostAI : MonoBehaviour
{
    // Definisikan state apa saja yang dimiliki hantu
    public enum GhostState { Patrol, Chase }
    
    [Header("Status Saat Ini")]
    public GhostState currentState;

    [Header("Pengaturan AI")]
    public Transform player;
    public float speed = 3f;
    public float chaseDistance = 5f; // Jarak pandang hantu

    private void Start()
    {
        // Set state awal
        currentState = GhostState.Patrol; 
        
        // Cari objek Pac-Man otomatis berdasarkan Tag
        if (player == null)
        {
            GameObject pacMan = GameObject.FindGameObjectWithTag("Player");
            if (pacMan != null) player = pacMan.transform;
        }
    }

    private void Update()
    {
        // Switch Case yang memisahkan logika tiap state dengan rapi
        switch (currentState)
        {
            case GhostState.Patrol:
                PatrolBehavior();
                CheckForPlayer();
                break;
                
            case GhostState.Chase:
                ChaseBehavior();
                CheckDistance();
                break;
        }
    }

    private void PatrolBehavior()
    {
        // Logika saat patroli. Sementara kita biarkan hantunya diam.
    }

    private void ChaseBehavior()
    {
        // Kejar Pac-Man! (Bergerak menuju posisi player)
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void CheckForPlayer()
    {
        // Transisi: Kalau jarak Pac-Man masuk area pandang, ubah state jadi Chase
        if (player != null && Vector2.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = GhostState.Chase;
            Debug.Log("Pac-Man terlihat! Hantu ganti state ke CHASE.");
        }
    }

    private void CheckDistance()
    {
        // Transisi: Kalau Pac-Man berhasil kabur jauh, balik lagi ke Patrol
        if (player != null && Vector2.Distance(transform.position, player.position) > chaseDistance)
        {
            currentState = GhostState.Patrol;
            Debug.Log("Pac-Man hilang. Hantu ganti state ke PATROL.");
        }
    }
}