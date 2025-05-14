using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseInit : MonoBehaviour {

    private static bool Initialized = false;
    private List<string> AllCollectables = new List<string> {
        "Book", "Gun",
        "FirstMemory", "SecondMemory",
        "PharmCard", "MedicKey", "StorageCard",
        "FirstDuckPaper", "SecondDuckPaper", "ThirdDuckPaper"
    };
    public List<string> currentRequiredPlayerItems = new List<string>();
    public int currentRequiredBookPages = 0;

    public MonoBehaviour runOnInit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        if (!Initialized) {
            foreach (string collectable in AllCollectables) {
                if (!PlayerPrefs.HasKey(collectable)) {
                    if ((currentRequiredPlayerItems.Contains(collectable))) {
                        PlayerPrefs.SetInt(collectable, 1);
                    } else {
                        PlayerPrefs.SetInt(collectable, 0);
                    }
                }
            }
            if (!PlayerPrefs.HasKey("BookPages") || PlayerPrefs.GetInt("BookPages") < currentRequiredBookPages) {
                PlayerPrefs.SetInt("BookPages", currentRequiredBookPages);
            }
            PlayerPrefs.SetString("CurrentUI", "None");
            Initialized = true;
        }
    }

    void Start() {
        if (runOnInit != null) {
            runOnInit.Invoke("Run", 0);
        }
    }
}
