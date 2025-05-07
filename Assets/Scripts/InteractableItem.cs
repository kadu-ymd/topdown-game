using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour {
    public ItemDisplayManager itemDisplay;
    public string collectableItemName;
    public bool destroyOnCollect = false;
    public string onIteractionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (!string.IsNullOrEmpty(collectableItemName)) {
            if (!PlayerPrefs.HasKey(collectableItemName)) {
                PlayerPrefs.SetInt(collectableItemName, 0);
                PlayerPrefs.Save(); 
            } else if (PlayerPrefs.GetInt(collectableItemName) == 1 && destroyOnCollect) {
                Destroy(gameObject); 
            }
        }
    }

    void Interact() {
        itemDisplay.EnterDisplay(onIteractionText);
        if (!string.IsNullOrEmpty(collectableItemName) && PlayerPrefs.GetInt(collectableItemName) == 0) {
            PlayerPrefs.SetInt(collectableItemName, 1);
            PlayerPrefs.Save(); 

            if (destroyOnCollect) Destroy(gameObject);
        }
    }
}
