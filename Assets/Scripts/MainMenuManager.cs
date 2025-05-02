using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("InitialRoom");
    }

    // tela de configurações

    public void QuitGame()
    {
        Application.Quit();
    }
}
