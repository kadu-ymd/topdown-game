using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightRotation : MonoBehaviour
{
    private FieldOfView parentFOV;
    private Light2D flashlight;
    private EnemyMoviment enemy;
    private bool turnedOff;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentFOV = GetComponentInParent<FieldOfView>();
        enemy = GetComponentInParent<EnemyMoviment>();
        flashlight = GetComponentInParent<Light2D>();
        CircleCollider2D circleCollider = GetComponentInParent<CircleCollider2D>();

        if (circleCollider != null) {
            flashlight.pointLightOuterRadius = circleCollider.radius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnedOff && enemy.sleeping) {
            flashlight.enabled = false;
            turnedOff = true;
        }
        else if (turnedOff && !enemy.sleeping) {
            flashlight.enabled = true;
            turnedOff = false;
        }

        if (parentFOV != null)
        {
            transform.localRotation = Quaternion.Euler(0, 0, parentFOV.angleRotation + 180);
        }
    }
}
