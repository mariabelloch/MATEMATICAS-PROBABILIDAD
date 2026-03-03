using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatePorTurnos : MonoBehaviour
{
    public enum Turno { Jugador, Enemigo }
    public Turno turnoActual = Turno.Jugador;

    [Header("Vida")]
    public float vidaJugador = 100f;
    public float vidaEnemigo = 100f;
    public float vidaMax = 100f;

    [Header("UI")]
    public Slider barraVidaJugador;
    public Slider barraVidaEnemigo;
    public TMP_Text textoTurno;

    [Header("Colores texto")]
    public Color colorMiss = Color.gray;
    public Color colorHit = Color.white;
    public Color colorCritico = Color.red;

    [Header("Jugador Stats")]
    [Range(0f, 1f)] public float precisionJugador = 0.8f;
    [Range(0f, 1f)] public float evasionEnemigo = 0.2f;
    [Range(0f, 1f)] public float critJugador = 0.2f;
    public float multCritJugador = 2f;
    public float danoMedioJugador = 20f;
    public float desviacionJugador = 3f;

    [Header("Enemigo Stats")]
    [Range(0f, 1f)] public float precisionEnemigo = 0.7f;
    [Range(0f, 1f)] public float evasionJugador = 0.15f;
    [Range(0f, 1f)] public float critEnemigo = 0.1f;
    public float multCritEnemigo = 1.8f;
    public float danoMedioEnemigo = 15f;
    public float desviacionEnemigo = 4f;

    private void Start()
    {
        ActualizarUI("Empieza el combate. Turno: Jugador", colorHit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EjecutarTurno();
        }
    }

    private void EjecutarTurno()
    {
        if (vidaJugador <= 0f || vidaEnemigo <= 0f)
            return;

        if (turnoActual == Turno.Jugador)
        {
            ProcesarAtaque(
                "Jugador",
                precisionJugador,
                evasionEnemigo,
                critJugador,
                multCritJugador,
                danoMedioJugador,
                desviacionJugador,
                ref vidaEnemigo
            );

            turnoActual = Turno.Enemigo;
        }
        else
        {
            ProcesarAtaque(
                "Enemigo",
                precisionEnemigo,
                evasionJugador,
                critEnemigo,
                multCritEnemigo,
                danoMedioEnemigo,
                desviacionEnemigo,
                ref vidaJugador
            );

            turnoActual = Turno.Jugador;
        }

        
        if (vidaEnemigo <= 0f)
            ActualizarUI(" Enemigo derrotado. ¡Ganas!", colorCritico);
        else if (vidaJugador <= 0f)
            ActualizarUI(" Has perdido.", colorCritico);
    }

    private void ProcesarAtaque(
        string atacante,
        float precision,
        float evasion,
        float probCrit,
        float multCrit,
        float danoMedio,
        float desviacion,
        ref float vidaObjetivo
    )
    {
        float pHit = Mathf.Clamp01(precision - evasion);

        if (!Probabilidad.Evento(pHit))
        {
            ActualizarUI($"{atacante}  MISS", colorMiss);
            return;
        }

        float dano = Probabilidad.GenerarValorNormalClampeado(danoMedio, desviacion, 0f, 999f);

        bool critico = Probabilidad.Evento(probCrit);
        if (critico)
        {
            dano *= multCrit;
            vidaObjetivo = Mathf.Clamp(vidaObjetivo - dano, 0f, vidaMax);
            ActualizarUI($"{atacante} CRÍTICO {dano:F1}", colorCritico);
        }
        else
        {
            vidaObjetivo = Mathf.Clamp(vidaObjetivo - dano, 0f, vidaMax);
            ActualizarUI($"{atacante}  HIT {dano:F1}", colorHit);
        }
    }

    private void ActualizarUI(string mensaje, Color color)
    {
        if (barraVidaJugador != null)
        {
            barraVidaJugador.minValue = 0;
            barraVidaJugador.maxValue = vidaMax;
            barraVidaJugador.value = vidaJugador;
        }

        if (barraVidaEnemigo != null)
        {
            barraVidaEnemigo.minValue = 0;
            barraVidaEnemigo.maxValue = vidaMax;
            barraVidaEnemigo.value = vidaEnemigo;
        }

        if (textoTurno != null)
        {
            textoTurno.text = mensaje;
            textoTurno.color = color;
        }

        Debug.Log(mensaje);
    }

    
    public void ReiniciarCombate()
    {
        vidaJugador = vidaMax;
        vidaEnemigo = vidaMax;
        turnoActual = Turno.Jugador;

        ActualizarUI("Combate reiniciado. Turno: Jugador", colorHit);
    }
}