using UnityEngine;

public class Pellet : MonoBehaviour
{    public AudioClip wakaSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika yang menyentuh pelet ini adalah Pac-Man
        if (collision.CompareTag("Player"))
        {
            // Mainkan suara waka-waka di posisi pelet ini berada
            if (wakaSound != null)
            {
                AudioSource.PlayClipAtPoint(wakaSound, transform.position);
            }

            // Lapor ke GameManager kalau pelet dimakan
            GameManager.instance.PelletEaten();
            
            // Hancurkan objek pelet ini
            Destroy(gameObject);
        }
    }
}