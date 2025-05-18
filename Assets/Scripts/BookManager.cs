using UnityEngine;
using UnityEngine.Audio;

public class BookManager : MonoBehaviour
{
    private static BookManager BookManagerInstance;
    private int pageCount;
    private GameObject pages;
    private AudioSource audioSource;
    private int maxPages = 1;
    private string exitDisplayText;

    void Awake() {
        if (BookManagerInstance == null) BookManagerInstance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pages = GameObject.Find("Folhas");
        pageCount = 1;
        SetActiveChildren(false);
        UpdatedeBookPages();
    }

    void Update()
    {
        string currentUI = PlayerPrefs.GetString("CurrentUI");
        if (currentUI == "Book")
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
                ExitDisplay();

            else if (Input.GetKeyDown(KeyCode.RightArrow))
                NextPage();

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                PrevPage();
        }
        else if (currentUI == "None")
        {
            if (Input.GetKeyDown(KeyCode.Q))
                EnterDisplay();
        }

    }

    public void ToPage(int page)
    {
        if (page > 0 && page <= maxPages)
        {
            pageCount = page;
            foreach (Transform folha in pages.transform)
            {
                if (folha.name == "Folha" + pageCount)
                {
                    folha.gameObject.SetActive(true);
                    audioSource.Play();
                }
                else
                    folha.gameObject.SetActive(false);
            }
        }
    }

    public void PrevPage()
    {
        ToPage(pageCount - 1);
    }

    public void NextPage()
    {
        ToPage(pageCount + 1);
    }

    public static int UpdatedeBookPages()
    {
        int newMaxPages = 0;
        if (PlayerPrefs.GetInt("SecondMemory") == 1)
            newMaxPages = 4;
        else if (PlayerPrefs.GetInt("FirstMemory") == 1)
            newMaxPages = 3;
        else if (PlayerPrefs.GetInt("Gun") == 1)
            newMaxPages = 2;
        else if (PlayerPrefs.GetInt("Book") == 1)
            newMaxPages = 1;
        PlayerPrefs.SetInt("BookPages", newMaxPages);
        BookManagerInstance.maxPages = newMaxPages;
        return newMaxPages;
    }

    public static void DisplayBookIntoPage(int page, string exitDisplayText = null)
    {
        Debug.Log(exitDisplayText);
        BookManagerInstance.ToPage(page);
        BookManagerInstance.EnterDisplay(exitDisplayText);
    }

    public void EnterDisplay(string exitDisplayText = null)
    {
        MenuManager.HidePauseButton();
        SetActiveChildren(true);
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "Book");

        if (!string.IsNullOrEmpty(exitDisplayText))
            this.exitDisplayText = exitDisplayText;
        else
            this.exitDisplayText = null;
    }

    public void ExitDisplay()
    {
        MenuManager.ShowPauseButton();
        SetActiveChildren(false);
        Time.timeScale = 1f;
        PlayerPrefs.SetString("CurrentUI", "None");

        if (!string.IsNullOrEmpty(exitDisplayText))
            ThoughtManager.ShowThought(exitDisplayText);
        exitDisplayText = null;
    }
    
    public void SetActiveChildren(bool active) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(active);
    }
}
}
