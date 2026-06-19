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

        // Nggak bisa jalan diagonal
        if (movement.x != 0) 
        {
            movement.y = 0; 
        }
    }

    private void FixedUpdate()
    {
        // Eksekusi gerakannya ke fisik Rigidbody
        rb.linearVelocity = movement * speed;
    }
}