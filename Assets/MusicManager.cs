using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic musicManagerInstance;

    void Awake()
    {
        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
            DontDestroyOnLoad(gameObject); // Mantém o objeto ativo em todas as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que não existam cópias extras
        }
    }
}
