using System.Collections.Generic;
using UnityEngine;

public class PelletPooler : MonoBehaviour
{
    // Gabungin dengan Singleton biar gampang dipanggil dari mana aja
    public static PelletPooler Instance { get; private set; } 
    
    public GameObject pelletPrefab;
    public int poolSize = 50; // Total pellet yang di-spawn di awal game

    private List<GameObject> pelletPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Bikin kolam (pool) pellet-nya
        pelletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pelletPrefab, this.transform);
            obj.SetActive(false); // Matiin dulu semuanya, biarin sembunyi
            pelletPool.Add(obj);
        }
    }

    // Fungsi buat ngambil pellet yang statusnya lagi mati/nganggur
    public GameObject GetPooledPellet()
    {
        for (int i = 0; i < pelletPool.Count; i++)
        {
            if (!pelletPool[i].activeInHierarchy)
            {
                return pelletPool[i];
            }
        }
        // Kalau butuh lebih banyak dari poolSize, dia balikin null
        return null; 
    }
}