using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombateArmaVsArmadura : MonoBehaviour
{
    [Header("Arma (daño normal)")]
    public float danoMedio = 20f;
    public float desviacion = 3f;
    public float danoMin = 1f;
    public float danoMax = 40f;

    [Header("Probabilidades")]
    [Range(0f, 1f)] public float probCritico = 0.2f;
    public float multiplicadorCritico = 2f;

    [Header("Arma vs Armadura (impacto)")]
    [Range(0f, 1f)] public float precisionArma = 0.8f;
    [Range(0f, 1f)] public float evasionEnemigo = 0.2f;

    [Header("UI (opcional)")]
    public Slider barraDanio;
    public TMP_Text textoResultado;

    private void Awake()
    {
        
        if (textoResultado == null)
        {
            
            TMP_Text[] textos = FindObjectsOfType<TMP_Text>(true);
            Debug.Log($"TMP_Text encontrados en escena: {textos.Length}");

            if (textos.Length > 0)
            {
                
                textoResultado = textos[0];
                Debug.Log($"Auto-asignado textoResultado = {textoResultado.name}");
            }
            else
            {
                Debug.LogError("No se ha encontrado ningún TMP_Text en la escena.");
            }
        }
    }

    private void Start()
    {
        Debug.Log("Start OK. textoResultado asignado? " + (textoResultado != null));
        EscribirTexto("TEST INICIAL");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Atacar();
        }
    }

    private void EscribirTexto(string msg)
    {
        if (textoResultado != null)
        {
            textoResultado.text = msg;
            textoResultado.ForceMeshUpdate();
            Debug.Log("Texto actualizado a: " + msg);
        }
        else
        {
            Debug.LogWarning("textoResultado es NULL");
        }
    }

    public void Atacar()
    {
        float probImpacto = Mathf.Clamp01(precisionArma - evasionEnemigo);

        bool impacta = Probabilidad.Evento(probImpacto);

        float dano = 0f;

        if (!impacta)
        {
            dano = 0f;
            EscribirTexto("MISS");
        }
        else
        {
            dano = Probabilidad.GenerarValorNormalClampeado(danoMedio, desviacion, danoMin, danoMax);

            bool critico = Probabilidad.Evento(probCritico);
            if (critico)
            {
                dano *= multiplicadorCritico;
                EscribirTexto("HIT CRÍTICO: " + dano.ToString("F2"));
            }
            else
            {
                EscribirTexto("HIT: " + dano.ToString("F2"));
            }
        }

        // Barra usando daño real (más claro visualmente)
        if (barraDanio != null)
        {
            barraDanio.minValue = danoMin;
            barraDanio.maxValue = danoMax;
            barraDanio.value = dano;
        }
    }
}