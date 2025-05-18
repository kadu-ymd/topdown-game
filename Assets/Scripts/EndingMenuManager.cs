using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenuManager : MonoBehaviour
{
    void Start()
    {
        // Limpa todos os PlayerPrefs, com exceção dos de volume
        List<string> prefsToKeep = new List<string> { "MasterVolume", "MusicVolume" };

        Dictionary<string, float> valoresPreservados = new Dictionary<string, float>();

        foreach (string chave in prefsToKeep)
        {
            if (PlayerPrefs.HasKey(chave))
            {
                valoresPreservados[chave] = PlayerPrefs.GetFloat(chave);
            }
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        foreach (KeyValuePair<string, float> entrada in valoresPreservados)
        {
            PlayerPrefs.SetFloat(entrada.Key, entrada.Value);
        }

        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("1Bedroom");
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
