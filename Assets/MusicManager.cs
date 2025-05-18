using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic musicManagerInstance;

    void Awake()
    {
        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
            DontDestroyOnLoad(gameObject); // Mant�m o objeto ativo em todas as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que n�o existam c�pias extras
        }
    }
}
