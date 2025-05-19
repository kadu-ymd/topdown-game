using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NecroteriumSceneManager : SceneInit
{
    public GameObject wife;
    private List<string> Texts = new List<string> { };
    private List<bool> originTexts = new List<bool> { }; // 1 = Player, 0 = Wife
    private int currentText = 0;

    protected override void RunInitialization()
    {
        if (!PlayerPrefs.HasKey("History"))
            PlayerPrefs.SetInt("History", 0);

        if (PlayerPrefs.GetInt("History") == 0)
        {
            if (PlayerPrefs.GetInt("TotalKills") <= 1)
            {
                Texts = new List<string>
                    {
                        "Quem é você? ... O que é você?", // Player
                        "Eu sou parte do passado que você esqueceu...",
                        "... ... ...", // Player
                        "Você me tirou deste mundo, mas fico feliz que, mesmo sem suas memórias, você não esqueceu o que sentia por mim.",
                        "Você trilhou um caminho sem sangue, recusando-se a ser o monstro que achavam que você fosse.",
                        "Talvez, então, ainda haja salvação para você.",
                        "Vá e descubra se é possível recomeçar, se há redenção para aqueles que erraram.",
                        "Mas saiba... algumas cicatrizes nunca desaparecem.",
                        "O que aconteceu com você? ... Com nós?", // Player
                        "O passado deve permanecer no passado.",
                        "Você não pode mudar o que já aconteceu. Mas você pode mudar quem vai ser daqui em diante.",
                        "Adeus... e boa sorte.",
                        "Espere! ... Espere! ...", // Player
                    };
                originTexts = new List<bool>
                    {
                        true, false, true, false, false, false, false, false, true, false, false, false, true
                    };

                GhostSpawner[] ghostSpawners = FindObjectsOfType<GhostSpawner>();
                foreach (GhostSpawner ghostSpawner in ghostSpawners)
                {
                    ghostSpawner.peaceful = true;
                }
            }
            else if (PlayerPrefs.GetInt("TotalKills") <= 3)
            {
                Texts = new List<string>
                    {
                        "Quem é você? ... O que é você?", // Player
                        "Eu sou parte do passado que você quase esqueceu...",
                        "... Não me diga que ...", // Player
                        "Eu estava lá quando sua mão segurou a arma.",
                        "Eu senti o frio atravessar meu peito.",
                        "Mas agora... agora você não lembra. Talvez seja melhor assim.",
                        "Você não é completamente perdido, mas também não se encontrou.",
                        "Você vagou entre a culpa e a violência, sem nunca escolher de verdade quem deseja ser",
                        "Olhe ao seu redor... os mortos que te perseguem foram resultado do seu caminho.",
                        "Me diga, amor... foi isso que você quis?",
                        "... Espera! ... ME DESCULPA! ...", // Player
                    };
                originTexts = new List<bool>
                    {
                        true, false, true, false, false, false, false, false, false, false, true
                    };
            }
            else
            {
                Texts = new List<string>
                    {
                        "Quem é você? ... O que é você?", // Player
                        "Eu era seu mundo, e ainda assim você me destruiu...",
                        "O-o que?", // Player
                        "Eles apagaram suas memórias, mas será que isso realmente te mudou?",
                        "Ou isso apenas revelou o que você sempre foi?",
                        "Seu caminho está pintado com sangue, e agora os mortos não te deixarão em paz.",
                        "Eu não posso impedir o que está por vir, mas posso te perguntar uma última vez: ...",
                        "você realmente acredita que foi forçado a isso, ou foi escolha sua?",
                        "E-espera! ... VOLTA!?", // Player
                        "Talvez aquele trágico acidente, não tenha sido tão acidental assim.",
                    };
                originTexts = new List<bool>
                    {
                        true, false, true, false, false, false, false, false, true, false
                    };
            }
            StartCoroutine(FlowDialog());
        }
        else
        {
            EnableSpawns();
            StartCoroutine(FadeOut());
        }

    }

    IEnumerator FlowDialog()
    {
        while (currentText < Texts.Count)
        {
            if (originTexts[currentText])
                ThoughtManager.ShowThought(Texts[currentText]);
            else
                WifeManager.ShowThought(Texts[currentText]);

            currentText++;
            while (PlayerPrefs.GetString("CurrentUI") != "None")
                yield return null;
        }
        PlayerPrefs.SetInt("History", 1);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        SpriteRenderer wifeSpriteRenderer = wife.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        Color originalColor = wifeSpriteRenderer.color;
        Color color = originalColor;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(originalColor.a, 0f, elapsedTime / 1f);
            wifeSpriteRenderer.color = color;
            yield return null;
        }
        EnableSpawns();
        Destroy(wife);
    }

    public void EnableSpawns()
    {
        GhostSpawner[] ghostSpawners = FindObjectsOfType<GhostSpawner>();
        foreach (GhostSpawner ghostSpawner in ghostSpawners)
        {
            ghostSpawner.canSpawn = true;
        }
    }
}
