using UnityEngine;

public class CorridorSceneManager : SceneInit
{
    protected override void RunInitialization()
    {
        int deaths = PlayerPrefs.GetInt("Deaths_" + sceneName);

        if (deaths == 0)
            ThoughtManager.ShowThought("Porque a luz está piscando? ... Enfim, eu deveria andar logo e sair desse lugar!");
        else if (deaths == 1)
            ThoughtManager.ShowThought("Como eu voltei para cá!? Eu não lembro exatamente o que aconteceu depois que aquele guarda me pegou, mas acho que não posso deixar isso acontecer de novo...");
        else if (deaths == 2)
            ThoughtManager.ShowThought("Parece que aquele guarda está cochilando... Se eu for um pouco mais furtivo eu consigo passar...");
        else if (deaths == 3)
            ThoughtManager.ShowThought("Parece que quando a lanterna dele está apagada, ele não me vê... Mas quando ela está acesa, eu sou um alvo fácil...");
        else 
            ThoughtManager.ShowThought("Droga! Fui pego de novo!");
    }
}
