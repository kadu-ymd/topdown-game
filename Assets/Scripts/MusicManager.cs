using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager musicManagerInstance;

    void Awake()
    {
        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
            DontDestroyOnLoad(gameObject); // Mantém o objeto ativo em todas as cenas
        }
        else
        {
            Destroy(gameObject); // Evita múltiplas instâncias
        }
    }
}
