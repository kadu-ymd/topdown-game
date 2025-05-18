using UnityEngine;
using UnityEngine.UI;

public class AlertBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    private EnemyMoviment enemy;

    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyMoviment>();
    }

    void Update()
    {
        float atention = Mathf.Clamp01(enemy.atention_level / enemy.maxAttention);
        slider.value = atention;

        SetColor(atention);

        if (atention < 0.05f)
        {
            slider.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (!slider.gameObject.activeSelf)
                slider.gameObject.SetActive(true);
        }
    }

    void SetColor(float atention)
    {
        Color targetColor;

        if (atention < 0.5f)
        {
            // Vermelho → Amarelo
            float lerpFactor = atention / 0.5f;
            targetColor = Color.Lerp(Color.green, Color.yellow, lerpFactor);
        }
        else
        {
            // Amarelo → Verde
            float lerpFactor = (atention - 0.5f) / 0.5f;
            targetColor = Color.Lerp(Color.yellow, Color.red, lerpFactor);
        }

        fillImage.color = targetColor;
    }
}
