using UnityEngine;

public class GeneradorHorda : MonoBehaviour
{
    [Header("Configuración de la Horda")]
    [Tooltip("Arrastra aquí el Prefab del Cubo Enemigo")]
    public GameObject enemigoPrefab;

    [Tooltip("Cada cuántos segundos aparece un nuevo enemigo")]
    public float tiempoEntreEnemigos = 2f;

    [Header("Zonas de Aparición")]
    [Tooltip("Arrastra aquí los objetos vacíos desde donde saldrán los enemigos")]
    public Transform[] puntosDeAparicion;

    void Start()
    {
        // Iniciar la creación de enemigos repitiendo la función cada X segundos
        InvokeRepeating("CrearEnemigo", 1f, tiempoEntreEnemigos);
    }

    void CrearEnemigo()
    {
        // Evitar errores si el alumno olvidó poner puntos de aparición
        if (puntosDeAparicion.Length == 0) return;

        // Elegir un punto al azar de la lista
        int puntoAleatorio = Random.Range(0, puntosDeAparicion.Length);
        Transform puntoElegido = puntosDeAparicion[puntoAleatorio];

        // Crear al enemigo en ese punto
        Instantiate(enemigoPrefab, puntoElegido.position, puntoElegido.rotation);
    }
}
