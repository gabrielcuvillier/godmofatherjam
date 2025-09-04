using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    [ContextMenu("Show Game Over UI")]
    public void ShowGameOverUI()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        GameManager.Instance.EnableCursor();
    }
}
