using System;
using UnityEngine;

// Pakai class statis biar gampang diakses sebagai pusat siaran (Publisher)
public static class GameEvents 
{
    // Event kalau Pac-Man makan Pellet (ngirim data skor)
    public static event Action<int> OnPelletEaten;
    
    // Event kalau Pac-Man mati atau game over
    public static event Action OnGameOver;

    // Fungsi ?.Invoke() dipakai biar nggak error kalau belum ada yang subscribe
    public static void TriggerPelletEaten(int scoreValue) => OnPelletEaten?.Invoke(scoreValue);
    
    public static void TriggerGameOver() => OnGameOver?.Invoke();
}