using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class EntranceSceneManager : SceneInit
{

    public TMP_Text patientRecordPasswordNumber;
    public Pinpad pinpad;
    public GameObject hearts;

    protected override void RunInitialization()
    {

        PlayerPrefs.SetInt("SecondMemory", 0);
        PlayerPrefs.SetInt("MedicKey", 0);

        int firstPasswordDigit = 5;
        int secondPasswordDigit = Random.Range(1, 10);
        int thirdPasswordDigit = Random.Range(0, 10);

        // Mudando número do prontuário
        patientRecordPasswordNumber.text = thirdPasswordDigit.ToString();
        
        // Mudando a quantidade de corações no post-it
        foreach (Transform heart in hearts.transform)
        {
            int number = 0;
            Match match = Regex.Match(heart.name, @"\d+");
            if (match.Success && int.TryParse(match.Value, out number))
            {
                if (number > secondPasswordDigit)
                {
                    heart.gameObject.SetActive(false);
                }
            }
        }

        // Alterando a senha no display
        pinpad.answer = firstPasswordDigit.ToString() + secondPasswordDigit.ToString() + thirdPasswordDigit.ToString();

        int deaths = PlayerPrefs.GetInt("Deaths_" + sceneName);

        if (deaths > 0)
            ThoughtManager.ShowThought("Voltei para o começo como nas ultimas salas... Mas sinto que algo mudou...");

    }
}
