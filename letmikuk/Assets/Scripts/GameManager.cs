using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    private int currentPellets;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetTotalPellets(int count)
    {
        currentPellets = count;
    }

    public void PelletEaten()
    {
        currentPellets--;
        // Tambah skor di sini kalau ada...

        // Cek kalau pellet habis
        if (currentPellets <= 0)
        {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Menang! Lanjut ke level berikutnya...");
        // Memuat level selanjutnya berdasarkan urutan di Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Cek apakah level selanjutnya ada, kalau mentok balik ke Main Menu (Index 0)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0); 
        }
    }
}