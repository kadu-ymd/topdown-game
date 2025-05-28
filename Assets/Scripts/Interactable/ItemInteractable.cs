using UnityEngine;
using TMPro;

public class ItemInteractable : MonoBehaviour {
    public ItemDisplayManager itemDisplay;
    public string collectableItemName;
    public bool destroyOnCollect = false;
    public string onIteractionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (itemDisplay == null && !string.IsNullOrEmpty(collectableItemName))  {
            Debug.LogError("ItemDisplayManager não atribuído. A interação não funcionará corretamente.");
            return;
        }
        if (!string.IsNullOrEmpty(collectableItemName)) {
            if (!PlayerPrefs.HasKey(collectableItemName)) {
                PlayerPrefs.SetInt(collectableItemName, 0);
                PlayerPrefs.Save(); 
            } else if (PlayerPrefs.GetInt(collectableItemName) == 1 && destroyOnCollect) {
                Destroy(gameObject); 
            }
        }
    }

    protected void Interact()
    {
        if (!string.IsNullOrEmpty(collectableItemName))
        {
            if (PlayerPrefs.GetInt(collectableItemName) == 0)
            {
                PlayerPrefs.SetInt(collectableItemName, 1);
                itemDisplay.EnterDisplay(onIteractionText);
                if (destroyOnCollect) Destroy(gameObject);
            }
            else
                ThoughtManager.ShowThought("Não há mais nada de importante aqui.");
        }
        else
            itemDisplay.EnterDisplay(onIteractionText);
    }
}
