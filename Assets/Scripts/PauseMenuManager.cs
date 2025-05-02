using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuUI.SetActive(false); // Hide the pause menu
        animator.enabled = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
        pauseMenuUI.SetActive(true); // Show the pause menu
        animator.enabled = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("MainMenu");
    }
}
