using UnityEngine;
using UnityEngine.Audio;

public class BookManager : MonoBehaviour
{
    private int pageCount;
    private GameObject folhas;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        folhas = GameObject.Find("Folhas");
        pageCount = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && PlayerPrefs.GetString("CurrentUI") == "ItemDisplay")
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && PlayerPrefs.GetString("CurrentUI") == "ItemDisplay")
        {
            PrevPage();
        }
    }

    public void PrevPage()
    {
        if (pageCount > 1)
        {
            pageCount--;
        }

        foreach (Transform folha in folhas.transform)
        {
            if (folha.name == "Folha" + pageCount)
            {
                folha.gameObject.SetActive(true);
                audioSource.Play();
            }
            else
            {
                folha.gameObject.SetActive(false);
            }
        }
    }

    public void NextPage()
    {
        if (pageCount <= 3)
        {
            pageCount++;
        }

        foreach (Transform folha in folhas.transform)
        {
            if (folha.name == "Folha" + pageCount)
            {
                folha.gameObject.SetActive(true);
                audioSource.Play();
            }
            else
            {
                folha.gameObject.SetActive(false);
            }
        }
    }
}
