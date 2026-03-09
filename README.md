 Entrega 2ª Convocatoria
Opción A  Implementación de probabilidad
Repositorio del proyecto:
https://github.com/mariabelloch/MATEMATICAS-PROBABILIDAD.git
Implementación de probabilidad en el proyecto
He implementado un sistema de combate sencillo en Unity que utiliza distintos conceptos de probabilidad para simular un sistema de ataque entre un jugador y un enemigo. El objetivo ha sido aplicar diferentes distribuciones de probabilidad para el cálculo del daño y eventos probabilísticos para determinar si un ataque impacta o si es crítico. En concreto, se ha utilizado principalmente la distribución normal, además de una distribución uniforme para comparar el comportamiento de ambas.
La implementación se ha realizado en varios scripts del proyecto.

Funciones implementadas
GenerarValorNormal()
Esta función genera valores aleatorios siguiendo una distribución normal. μ (media): valor medio del daño / σ (desviación estándar): variación del daño alrededor de la media. Para que la mayoría de los valores generados se concentren cerca de la media, mientras que valores muy altos o muy bajos son menos probables. Esta función se utiliza para generar el daño base del ataque, creando variaciones en lugar de un daño uniforme.

GenerarValorNormalClampeado()
Esta función aplica la distribución normal pero limita el resultado a un rango mínimo y máximo mediante Clamp. Esto evita resultados extremos o valores negativos.

Evento(probabilidad)
Esta función implementa un evento probabilístico basado en una probabilidad entre 0 y 1. Internamente compara un valor aleatorio con la probabilidad indicada. Se utiliza para determinar si un ataque impacta o falla (HIT/MISS) o si un ataque es crítico


Distribuciones implementadas
Además de la distribución normal, se ha implementado una segunda forma de generación de daño basada en una distribución uniforme. En esta distribución todos los valores dentro del rango tienen la misma probabilidad de aparecer.
Cada personaje utiliza un tipo de arma diferente, lo que permite observar visualmente las diferencias entre ambas distribuciones durante el combate.

Sistema de combate
El sistema principal se encuentra implementado en el script CombatePorTurnos.cs. Este script simula un combate por turnos entre jugador y enemigo. Cada turno se ejecuta un ataque y se aplican distintos cálculos probabilísticos.









Funcionamiento del sistema
Cada vez que el jugador pulsa la tecla Espacio, se ejecuta un turno de combate. Primero se calcula la probabilidad de que el ataque impacte utilizando la precisión del atacante y la evasión del defensor. Si el evento probabilístico falla, el resultado es MISS.
Si el ataque impacta, el daño se genera mediante la distribución correspondiente al arma del atacante. Esto permite observar diferencias en el comportamiento del daño generado. Después de calcular el daño base, se evalúa si el ataque es crítico mediante otro evento probabilístico. Si ocurre un crítico, el daño se multiplica por un factor.
Se ha implementado también un botón de interfaz que permite reiniciar el combate, restaurando la vida del jugador y del enemigo.
Conclusión
He utilizado principalmente la distribución normal para generar daño de manera más natural y equilibrada, además de una distribución uniforme para comparar su comportamiento, así como eventos probabilísticos para simular mecánicas comunes en juegos como la probabilidad de impacto y la probabilidad de crítico
Además, el sistema incluye un registro de estadísticas que permite analizar experimentalmente los resultados obtenidos, lo que ayuda a comprender mejor el comportamiento de los sistemas probabilísticos aplicados.
