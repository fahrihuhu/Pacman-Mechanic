using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Ambil input dari keyboard (tombol WASD atau panah)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0) 
        {
            movement.y = 0; 
        }

        if (movement != Vector2.zero)
        {
            // Mencari sudut (angle) berdasarkan arah input
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            // Memutar objek Pac-Man sesuai sudut tersebut
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        // Eksekusi gerakannya ke fisik Rigidbody
        rb.linearVelocity = movement * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek apakah objek yang ditabrak punya Tag "Ghost"
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // Lapor kalau Pac-Man mati
            GameManager.instance.LoseGame();
        }
    }
}