using UnityEngine;

public class BookManager : MonoBehaviour
{
    private int pageCount;
    private GameObject folhas;

    void Start()
    {
        folhas = GameObject.Find("Folhas");
        pageCount = 1;
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
            }
            else
            {
                folha.gameObject.SetActive(false);
            }
        }

        Debug.Log(pageCount);
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
            }
            else
            {
                folha.gameObject.SetActive(false);
            }
        }

        Debug.Log(pageCount);
    }
}
