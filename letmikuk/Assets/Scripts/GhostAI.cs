using UnityEngine;
public class GhostAI : MonoBehaviour
{
private Rigidbody2D rb;

private Vector2 currentMoveDir; // Ingatan hantu sedang jalan ke arah mana
    public enum GhostState { Patrol, Chase }

    [Header("Status Saat Ini")]
    public GhostState currentState;

    [Header("Pengaturan AI")]
    public Transform player;
    public float speed = 3f;
    public float chaseDistance = 5f;

    [Header("Pengaturan Mata Hantu")]
    public SpriteRenderer leftEyeRenderer;  
    public SpriteRenderer rightEyeRenderer; 

    [Header("Sprite Mata Kiri (Biasanya berakhiran _0)")]
    public Sprite leftEyeUp;
    public Sprite leftEyeDown;
    public Sprite leftEyeLeft;
    public Sprite leftEyeRight;

    [Header("Sprite Mata Kanan (Biasanya berakhiran _1)")]
    public Sprite rightEyeUp;
    public Sprite rightEyeDown;
    public Sprite rightEyeLeft;
    public Sprite rightEyeRight;

    [Header("Pengaturan Sensor Tembok")]
    public LayerMask wallLayer;

    private void Start()
    {
        currentState = GhostState.Patrol;
        
        // Ambil komponen Rigidbody2D hantu
        rb = GetComponent<Rigidbody2D>();
        
        if (player == null)
        {
            GameObject pacMan = GameObject.FindGameObjectWithTag("Player");
            if (pacMan != null) player = pacMan.transform;
        }
    }

    private void Update()
    {
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
        rb.linearVelocity = Vector2.zero;  
    }

   private void ChaseBehavior()
    {
        if (player == null) return;

        // 1. Kalau baru nge-spawn atau mentok tembok depan, cari jalan baru
        if (currentMoveDir == Vector2.zero || !IsDirectionClear(currentMoveDir))
        {
            ChooseNewDirection();
        }
        else
        {
            // 2. Kalau lagi jalan lurus, aktifkan "Mata Samping" untuk ngecek persimpangan!
            
            // Jika hantu bergerak Horizontal (Kiri/Kanan)
            if (currentMoveDir.x != 0)
            {
                // Cek apakah posisi Pac-Man ada di Atas / Bawah secara signifikan
                float yDiff = player.position.y - transform.position.y;
                if (Mathf.Abs(yDiff) > 0.3f) 
                {
                    Vector2 checkDir = new Vector2(0, Mathf.Sign(yDiff));
                    // Jika lorong pintasan ke atas/bawah KOSONG, langsung belok!
                    if (IsDirectionClear(checkDir)) currentMoveDir = checkDir;
                }
            }
            // Jika hantu bergerak Vertikal (Atas/Bawah)
            else if (currentMoveDir.y != 0)
            {
                // Cek apakah posisi Pac-Man ada di Kiri / Kanan
                float xDiff = player.position.x - transform.position.x;
                if (Mathf.Abs(xDiff) > 0.3f)
                {
                    Vector2 checkDir = new Vector2(Mathf.Sign(xDiff), 0);
                    // Jika lorong pintasan ke kiri/kanan KOSONG, langsung belok!
                    if (IsDirectionClear(checkDir)) currentMoveDir = checkDir;
                }
            }
        }

        // 3. Eksekusi pergerakan fisika
        rb.linearVelocity = currentMoveDir * speed;
        UpdateEyes(currentMoveDir);
    }

    // Fungsi baru untuk mikir saat mentok tembok
    private void ChooseNewDirection()
    {
        Vector2[] possibleDirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 bestDir = Vector2.zero;
        float shortestDist = float.MaxValue;

        foreach (Vector2 dir in possibleDirs)
        {
            // ATURAN EMAS: Haram hukumnya putar balik 180 derajat (kecuali awal mulai)
            if (currentMoveDir != Vector2.zero && dir == -currentMoveDir) continue; 

            // Cek apakah jalannya ngga ada tembok
            if (IsDirectionClear(dir))
            {
                // Simulasi jarak dari titik belokan itu ke Pac-Man
                float dist = Vector2.Distance((Vector2)transform.position + dir, player.position);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    bestDir = dir;
                }
            }
        }

        // Kalau masuk jalan buntu total (Dead End), baru terpaksa putar balik
        if (bestDir == Vector2.zero) bestDir = -currentMoveDir;

        currentMoveDir = bestDir;
    }

    private void UpdateEyes(Vector2 dir)
    {
        // Pastikan kedua mata sudah diisi di Inspector
        if (leftEyeRenderer == null || rightEyeRenderer == null) return;

        if (dir.x > 0) // Gerak Kanan
        {
            leftEyeRenderer.sprite = leftEyeRight;
            rightEyeRenderer.sprite = rightEyeRight;
        }
        else if (dir.x < 0) // Gerak Kiri
        {
            leftEyeRenderer.sprite = leftEyeLeft;
            rightEyeRenderer.sprite = rightEyeLeft;
        }
        else if (dir.y > 0) // Gerak Atas
        {
            leftEyeRenderer.sprite = leftEyeUp;
            rightEyeRenderer.sprite = rightEyeUp;
        }
        else if (dir.y < 0) // Gerak Bawah
        {
            leftEyeRenderer.sprite = leftEyeDown;
            rightEyeRenderer.sprite = rightEyeDown;
        }
    }

    private void CheckForPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = GhostState.Chase;
        }
    }

    private void CheckDistance()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) > chaseDistance)
        {
            currentState = GhostState.Patrol;
        }
    }

    private bool IsDirectionClear(Vector2 dir)
    {
        // Radius bola dikecilkan jadi 0.1f (agar tidak nyenggol tembok samping di lorong sempit)
        // Jarak pandang dipendekkan jadi 0.25f (agar tidak nembus deteksi tembok di lorong seberang)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, dir, 0.25f, wallLayer);
        return hit.collider == null; 
    }
}