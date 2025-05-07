using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ExitManager : MonoBehaviour {

    public List<string> RequiredItemsName = new List<string>();
    public string nextSceneName;

    void Awake() {
        if (SceneManager.GetActiveScene().name == "InitialRoom") {
            PlayerPrefs.DeleteAll();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (!Application.CanStreamedLevelBeLoaded(nextSceneName)) {
            Debug.LogError("A cena " + nextSceneName + " não existe ou não está incluída na Build Settings.");
            return;
        }
        foreach (string item in RequiredItemsName) {
            if (!PlayerPrefs.HasKey(item)) {
                PlayerPrefs.SetInt(item, 0);
            }
        }
        PlayerPrefs.Save();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            foreach (string item in RequiredItemsName) {
                if (PlayerPrefs.GetInt(item) == 0) {
                    ThoughtManager.ShowThought("Acho melhor eu vasculhar mais o lugar antes de sair...");
                    return;
                }
            }
            SceneManager.LoadSceneAsync(nextSceneName);      
        }
    }
}
