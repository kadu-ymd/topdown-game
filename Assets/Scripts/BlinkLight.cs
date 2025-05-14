using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class BlinkLight : MonoBehaviour
{
    public float lightOnIntensity = 0.8f;
    public float lightOffIntensity = 0.2f;

    public float minOnTime = 0.5f;
    public float maxOnTime = 2f;

    public float minOffTime = 0.1f;
    public float maxOffTime = 0.4f;

    private Light2D globalLight;
    private float timer;

    private bool isLightOn = true;
    private float nextChangeTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalLight = GetComponent<Light2D>();
        
        globalLight.intensity = lightOnIntensity;
        SetRandomChangeTime();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextChangeTime)
        {
            isLightOn = !isLightOn;
            globalLight.intensity = isLightOn ? lightOnIntensity : lightOffIntensity;
            SetRandomChangeTime();
            timer = 0f;
        }
        
    }

    void SetRandomChangeTime()
    {
        if (isLightOn)
        {
            nextChangeTime = Random.Range(minOnTime, maxOnTime);
        }
        else
        {
            nextChangeTime = Random.Range(minOffTime, maxOffTime);
        }
    }
}
