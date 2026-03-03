using UnityEngine;

public static class Probabilidad
{
    public static float GenerarValorNormal(float media, float desviacion)
    {
        if (desviacion < 0f) desviacion = -desviacion;

        float u1 = 1f - Random.value;
        float u2 = 1f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);

        return media + desviacion * randStdNormal;
    }

    public static float GenerarValorNormalClampeado(float media, float desviacion, float min, float max)
    {
        float v = GenerarValorNormal(media, desviacion);
        return Mathf.Clamp(v, min, max); 
    }

    public static bool Evento(float probabilidad)
    {
        return Random.value < Mathf.Clamp01(probabilidad);
    }
}
