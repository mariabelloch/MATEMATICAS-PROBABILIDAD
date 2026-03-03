using UnityEngine;
using UnityEngine.UI;

public class DanoArma : MonoBehaviour
{
    [Header("Daño base")]
    public float danomedio = 20f;
    public float danodesviacion = 3f;

    [Header("Limites")]
    public float minDano = 1.0f;
    public float maxDano = 999f;

    [Header("UI")]
    public Slider barraDano;


    public float CalcularDano()
    {
        float dano = Probabilidad.GenerarValorNormalClampeado
            ( danomedio,  danodesviacion, minDano, maxDano);

        return dano;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float d = CalcularDano();

            float t = Mathf.InverseLerp(minDano, maxDano, d);

            if (barraDano != null)
                barraDano.value = t;


            Debug.Log($"Daño generado Normal: {d:F2} (Barra: {t:F2})");
        }
    }
}
