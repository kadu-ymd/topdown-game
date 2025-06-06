using UnityEngine;

public class StaffAreaSceneManager : SceneInit
{
    protected override void RunInitialization()
    {

        PlayerPrefs.SetInt("SecondMemory", 0);
        PlayerPrefs.SetInt("StaffKey", 0);

        ItemPossiblyInteractable[] itemPlaces = FindObjectsOfType<ItemPossiblyInteractable>();
        int choice = Random.Range(0, itemPlaces.Length);
        itemPlaces[choice].haveItem = true;

        // int deaths = PlayerPrefs.GetInt("Deaths_" + sceneName);
    }
}
