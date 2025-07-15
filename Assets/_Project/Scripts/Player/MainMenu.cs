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

    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
