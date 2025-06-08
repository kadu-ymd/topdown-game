using UnityEngine;
using UnityEngine.Audio;

public class BookManager : ItemDisplayManager
{
    private static BookManager BookManagerInstance;
    private int pageCount;
    private GameObject pages;
    private AudioSource audioSource;
    private int maxPages = 1;

    void Awake() {
        if (BookManagerInstance == null) BookManagerInstance = this;
    }

    protected override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pages = GameObject.Find("Folhas");
        pageCount = 1;
        UpdatedeBookPages();
        SetActiveChildren(false);
    }

    protected override void Update()
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
        else if (currentUI == "None" && PlayerPrefs.GetInt("Book") == 1)
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

            if (pages != null)
            {
                foreach (Transform folha in pages.transform)
                {
                    if (folha.name == "Folha" + pageCount)
                        folha.gameObject.SetActive(true);
                    else
                        folha.gameObject.SetActive(false);
                }
            }
            else
                Debug.LogError("Folhas GameObject not found in BookManager.");
        }
    }

    public void PrevPage()
    {
        audioSource.Play();
        ToPage(pageCount - 1);
    }

    public void NextPage()
    {
        audioSource.Play();
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

    public static void OpenBook(string exitDisplayText = null)
    {
        BookManagerInstance.EnterDisplay(exitDisplayText);
    }

    public override void EnterDisplay(string exitDisplayText = null)
    {
        UpdatedeBookPages();
        BookManagerInstance.ToPage(BookManagerInstance.maxPages);
        MenuManager.HidePauseButton();
        BookManagerInstance.SetActiveChildren(true);
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "Book");

        if (!string.IsNullOrEmpty(exitDisplayText))
            BookManagerInstance.exitDisplayText = exitDisplayText;
        else
            BookManagerInstance.exitDisplayText = null;
    }

    public override void ExitDisplay()
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
