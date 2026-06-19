using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int scoreValue = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang nabrak itu Pac-Man (menggunakan Tag)
        if (collision.CompareTag("Player"))
        {
            // Kasih tahu GameManager kalau pellet ini dimakan
            GameEvents.TriggerPelletEaten(scoreValue);
            
            gameObject.SetActive(false);
        }
    }
}