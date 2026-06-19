using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pengaturan Grid")]
    public int columns = 10;
    public int rows = 5;
    public float spacing = 1.0f; // Jarak antar pellet

    private void Start()
    {
        SpawnPellets();
    }

    private void SpawnPellets()
    {
        // Titik awal penyebaran (biar pellet-nya posisinya di tengah layar)
        Vector2 startPos = new Vector2(-columns * spacing / 2f, -rows * spacing / 2f);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Minta 1 Pellet yang lagi nganggur
                GameObject pellet = PelletPooler.Instance.GetPooledPellet();
                
                if (pellet != null)
                {
                    // Atur posisinya biar menyebar dalam bentuk Grid (kotak-kotak)
                    pellet.transform.position = startPos + new Vector2(x * spacing, y * spacing);
                    
                    // munculin ke layar
                    pellet.SetActive(true);
                }
            }
        }
    }
}