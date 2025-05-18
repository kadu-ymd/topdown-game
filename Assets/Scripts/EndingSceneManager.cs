using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndingSceneManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    private GameObject goodBGImage;
    private GameObject badBGImage;
    private GameObject neutralBGImage;

    private int killCount;

    private void Start()
    {
        goodBGImage = GameObject.Find("GoodEndingImage");
        badBGImage = GameObject.Find("BadEndingImage");
        neutralBGImage = GameObject.Find("NeutralEndingImage");

        goodBGImage.SetActive(false);
        badBGImage.SetActive(false);
        neutralBGImage.SetActive(false);

        SetKillCount(3);

        killCount = PlayerPrefs.GetInt("TotalKills");

        Color badEndingColor = new Color(255, 255, 255);
        Color goodEndingColor = new Color(0, 0, 0);
        Color neutralEndingColor = new Color(0, 0, 0);

        if (killCount == 1)
        {
            goodBGImage.SetActive(true);

            SetText(title, "Um novo come�o", goodEndingColor);
            SetText(description, "Voc� se redimiu.", goodEndingColor);
        }
        else if (killCount > 1 && killCount < 5)
        {
            neutralBGImage.SetActive(true);

            SetText(title, "Neutro", neutralEndingColor);
            SetText(description, "Por melhor que fosse o sentimento, voc� resistiu a ele. Talvez ainda exista um pouco de humanidade em voc�.", neutralEndingColor);
        }
        else if (killCount >= 5)
        {
            badBGImage.SetActive(true);

            SetText(title, "Assassino", badEndingColor);
            SetText(description, "Mesmo perdendo suas mem�rias, o sentimento de matar � intr�nseco ao seu ser.", badEndingColor);
        }
        else
        {
            SetText(title, "Algo de errado...", goodEndingColor);
            SetText(description, "Como voc� veio parar aqui?", goodEndingColor);
        }

    }

    // Fun��o para testes -> n�o precisa ser chamada
    private void SetKillCount(int num)
    {
        PlayerPrefs.SetInt("TotalKills", num);
    }

    public void SetImage(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

    public void SetText(TextMeshProUGUI tmp, string message, Color textColor)
    {
        tmp.text = message;
        tmp.color = textColor;
    }
}