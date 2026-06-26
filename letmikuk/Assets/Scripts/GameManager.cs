using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Informasi Game")]
    private int currentPellets;
    private bool isGameOver = false; // Penting biar menangnya/kalahnya ngga ke-trigger dobel

    private void Awake()
    {
        // Singleton pattern agar GameManager mudah dipanggil
        if (instance == null) instance = this;
    }

    public void SetTotalPellets(int count)
    {
        currentPellets = count;
    }

    public void PelletEaten()
    {
        if (isGameOver) return; // Kalau udah game over, pelet jangan dihitung lagi

        currentPellets--;
        
        // Cek syarat MENANG: Kalau pelet habis
        if (currentPellets <= 0)
        {
            WinGame();
        }
    }

    // Fungsi ini dipanggil dari script Player (Pac-Man) saat nabrak hantu
    public void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        Debug.Log("Kalah! Dimakan Hantu!");

        // Panggil suara Game Over dari AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSFX();
        }
        
        // Mengulang (Restart) level yang sedang dimainkan saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        Debug.Log("Menang! Lanjut Level Selanjutnya!");
        
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            // Kembali ke Main Menu jika level sudah mentok
            SceneManager.LoadScene(0); 
        }
    }
}