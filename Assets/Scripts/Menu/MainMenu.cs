using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private Button playButton;
    private bool isLoading = false;

    public void StartGame()
    {
        if (isLoading) return;
        isLoading = true;

        // Désactive tous les boutons du menu
        playButton.interactable = false;

        // Lance le chargement asynchrone de la scène
        StartCoroutine(LoadGameSceneAsync());
    }

    private System.Collections.IEnumerator LoadGameSceneAsync()
    {
        var asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            // Met à jour le fillAmount de l'image de chargement
            loadingImage.fillAmount = asyncOp.progress < 0.9f ? asyncOp.progress / 0.9f : 1f;

            // Quand le chargement est terminé, active la scène
            if (asyncOp.progress >= 0.9f)
            {
                loadingImage.fillAmount = 1f;
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
