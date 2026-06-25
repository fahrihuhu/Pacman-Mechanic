using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pengaturan Arena")]
    public Tilemap wallMap;
    public GameObject pelletPrefab;

    private void Start()
    {
        GeneratePellets();
    }

    private void GeneratePellets()
    {
        int totalPellets = 0;

        // Unity otomatis mencari kotak terluar dari gambar labirin
        BoundsInt bounds = wallMap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            // Ambil posisi lokal ubin
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            // Hanya spawn pellet jika di posisi tersebut benar-benar kosong 
            // dan posisi tersebut berada di dalam batas area labirin yang digambar
            if (wallMap.HasTile(localPlace) == false)
            {
                // Cek lagi apakah ubin ini dikelilingi ubin lain biar gak spawn di luar map jauh
                Vector3 spawnPos = wallMap.GetCellCenterWorld(localPlace);

                // Batasi area spawn agar pellet gak meluber keluar batas luar tembok labirin
                if (spawnPos.x > -5f && spawnPos.x < 6f && spawnPos.y > -5f && spawnPos.y < 5f)
                {
                    Instantiate(pelletPrefab, spawnPos, Quaternion.identity, transform);
                    totalPellets++;
                }
            }
        }

        // Lapor jumlah total pellet ke GameManager biar sistem menangnya jalan
        if (GameManager.instance != null)
        {
            GameManager.instance.SetTotalPellets(totalPellets);
        }
    }
}