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

    [Header("Estadísticas")]
    public TMP_Text textoEstadisticas;
    private int ataquesTotales = 0;
    private int totalHits = 0;
    private int totalMiss = 0;
    private int totalCriticos = 0;

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

    public enum TipoArma
    {
        Uniforme,
        Normal
    }

    [Header("Tipo de arma")]
    public TipoArma armaJugador = TipoArma.Normal;
    public TipoArma armaEnemigo = TipoArma.Uniforme;

    private void Start()
    {
        ActualizarUI("Empieza el combate. Turno: Jugador", colorHit);
        ActualizarEstadisticas();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EjecutarTurno();
        }
    }

    private float GenerarDanio(TipoArma tipoArma, float danoMedio, float desviacion)
    {
        if (tipoArma == TipoArma.Uniforme)
        {
            return Random.Range(danoMedio - desviacion, danoMedio + desviacion);
        }
        else
        {
            return Probabilidad.GenerarValorNormalClampeado(danoMedio, desviacion, 0f, 999f);
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
                armaJugador,
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
                armaEnemigo,
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
            ActualizarUI("Enemigo derrotado. ¡Ganas!", colorCritico);
        else if (vidaJugador <= 0f)
            ActualizarUI("Has perdido.", colorCritico);
    }

    private void ProcesarAtaque(
        string atacante,
        TipoArma tipoArma,
        float precision,
        float evasion,
        float probCrit,
        float multCrit,
        float danoMedio,
        float desviacion,
        ref float vidaObjetivo
    )
    {
        ataquesTotales++;

        float pHit = Mathf.Clamp01(precision - evasion);

        if (!Probabilidad.Evento(pHit))
        {
            totalMiss++;
            ActualizarUI($"{atacante} ({tipoArma}) MISS", colorMiss);
            ActualizarEstadisticas();
            return;
        }

        float dano = GenerarDanio(tipoArma, danoMedio, desviacion);
        bool critico = Probabilidad.Evento(probCrit);

        totalHits++;

        if (critico)
        {
            totalCriticos++;
            dano *= multCrit;
            vidaObjetivo = Mathf.Clamp(vidaObjetivo - dano, 0f, vidaMax);
            ActualizarUI($"{atacante} ({tipoArma}) CRÍTICO {dano:F1}", colorCritico);
        }
        else
        {
            vidaObjetivo = Mathf.Clamp(vidaObjetivo - dano, 0f, vidaMax);
            ActualizarUI($"{atacante} ({tipoArma}) HIT {dano:F1}", colorHit);
        }

        ActualizarEstadisticas();
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

    private void ActualizarEstadisticas()
    {
        if (textoEstadisticas != null)
        {
            textoEstadisticas.text =
                "Ataques: " + ataquesTotales +
                "\nHits: " + totalHits +
                "\nMiss: " + totalMiss +
                "\nCríticos: " + totalCriticos;
        }
    }

    public void ReiniciarCombate()
    {
        vidaJugador = vidaMax;
        vidaEnemigo = vidaMax;
        turnoActual = Turno.Jugador;

        ataquesTotales = 0;
        totalHits = 0;
        totalMiss = 0;
        totalCriticos = 0;

        ActualizarUI("Combate reiniciado. Turno: Jugador", colorHit);
        ActualizarEstadisticas();
    }
}