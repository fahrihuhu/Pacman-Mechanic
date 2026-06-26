using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pengaturan Pellet Manual via Tilemap")]
    public Tilemap pelletMap;       
    public GameObject pelletPrefab; 
    public Transform pelletParent;  

    private void Start()
    {
        SpawnPelletsFromTilemap();
    }

    private void SpawnPelletsFromTilemap()
    {
        if (pelletMap == null || pelletPrefab == null) return;

        int totalPellets = 0;
        BoundsInt bounds = pelletMap.cellBounds;

        // Menyusuri hanya tempat yang kamu gambar saja!
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            // Jika di titik koordinat ini kamu ada menggambar sesuatu
            if (pelletMap.HasTile(localPlace))
            {
                // Ambil posisi koordinat dunia yang presisi di tengah ubin
                Vector3 spawnPos = pelletMap.GetCellCenterWorld(localPlace);

                // Spawn Prefab Pellet asli ke dalam folder pelindung biar Hierarchy rapi
                Instantiate(pelletPrefab, spawnPos, Quaternion.identity, pelletParent);
                totalPellets++;

                // Hapus gambar ubinnya dari layar biar gak numpuk dengan objek asli
                pelletMap.SetTile(localPlace, null);
            }
        }

        // Lapor jumlah pellet ke GameManager biar sistem menang bekerja
        if (GameManager.instance != null)
        {
            GameManager.instance.SetTotalPellets(totalPellets);
        }
    }
}