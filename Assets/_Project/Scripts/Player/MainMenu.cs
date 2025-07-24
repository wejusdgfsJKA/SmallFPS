using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Start from level 1.
    /// </summary>
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// Continue the game from the last level the player reached.
    /// </summary>
    public void ContinueGame()
    {
        GameManager.Instance.LoadProgress();
    }
    /// <summary>
    /// Quit the application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
