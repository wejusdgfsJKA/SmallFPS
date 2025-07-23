using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ContinueGame()
    {
        GameManager.Instance.LoadProgress();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
