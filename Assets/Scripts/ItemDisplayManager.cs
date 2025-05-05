using UnityEngine;

public class ItemDisplayManager : MonoBehaviour
{
    public GameObject itemDisplayPrefab;
    public GameObject pauseMenuPrefab;

    public void EnterDisplay()
    {
        pauseMenuPrefab.SetActive(false);
        itemDisplayPrefab.SetActive(true);
    }

    public void ExitDisplay()
    {
        pauseMenuPrefab.SetActive(true);
        itemDisplayPrefab.SetActive(false);
    }
}
