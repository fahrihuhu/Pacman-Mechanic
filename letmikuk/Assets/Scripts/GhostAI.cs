using UnityEngine;
public class GhostAI : MonoBehaviour
{
private Rigidbody2D rb;
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
        if (player != null)
        {
            // Cari tahu jarak antara hantu dan Pac-Man
            Vector2 direction = player.position - transform.position;
            Vector2 moveInput = Vector2.zero;

            // Kunci gerakan biar cuma vertikal atau horizontal
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                moveInput.x = Mathf.Sign(direction.x); 
            }
            else
            {
                moveInput.y = Mathf.Sign(direction.y); 
            }

            // Eksekusi gerakan
            rb.linearVelocity = moveInput * speed;
            // Update mata sesuai arah geraknya
            UpdateEyes(moveInput);
        }
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
}