using UnityEngine;

public class PharmHallSceneManager : SceneInit
{
    protected override void RunInitialization()
    {
        int deaths = PlayerPrefs.GetInt("Deaths_" + sceneName);

        PlayerPrefs.SetInt("FirstMemory", 0);

        if (deaths == 0)
            ThoughtManager.ShowThought("Outra sala... Pelo menos a luz não está piscando...");
        if (deaths == 1)
            ThoughtManager.ShowThought("Quem é aquele cara? Porque ele veio me perseguindo com tanta raiva? Ele parecia um pouco alterado...");
        else if (deaths > 1 && PlayerPrefs.GetInt("Gun") == 0)
            ThoughtManager.ShowThought("Acho que não vou conseguir passar desse cara só correndo, preciso de algo para me ajudar...");
        else if (deaths > 1 && PlayerPrefs.GetInt("Gun") == 1)
            ThoughtManager.ShowThought("... ... ... Não queria ter que usar a arma, mas não tenho escolha. Eu preciso sair logo daqui...");
    }
}
