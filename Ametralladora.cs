using UnityEngine;
using UnityEngine.UI;

public class Ametralladora : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject balaPrefab;      // La bolita que van a disparar
    public Transform puntoDeDisparo;   // De dónde sale la bala (la punta del cañón)
    public float fuerzaDisparo = 2000f;
    public float cadencia = 0.1f;      // Qué tan rápido dispara (0.1 = muy rápido)

    private float tiempoProximoDisparo = 0f;


    [Header("Sistema de Temperatura")]
    public float calorMaximo = 100f;
    public float calorPorDisparo = 10f;
    public float velocidadEnfriamiento = 30f;

    public float calorActual = 0f;
    public bool estaSobrecalentada = false;


    [Header("Interfaz de Usuario")]
    public Slider barraDeCalor;
    void Update()
    {

        // --- ESTO ES NUEVO (Bajar la temperatura) ---
        if (calorActual > 0)
        {
            calorActual -= velocidadEnfriamiento * Time.deltaTime;
            if (calorActual <= 0)
            {
                calorActual = 0;
                estaSobrecalentada = false; // Desbloquea el arma
            }
        }
        // --------------------------------------------

        // Si dejan presionado el clic izquierdo y ya pasó el tiempo de recarga
        if (!estaSobrecalentada && Input.GetButton("Fire1") && Time.time >= tiempoProximoDisparo)
        {
            tiempoProximoDisparo = Time.time + cadencia;
            Disparar();
        }

        if (barraDeCalor != null)
        {
            barraDeCalor.maxValue = calorMaximo; // La barra crece hasta el límite que tú le pongas
            barraDeCalor.value = calorActual;    // Rellena la barra según el calor del momento
        }
    }

    void Disparar()
    {
        // 1. Crear la bala
        GameObject bala = Instantiate(balaPrefab, puntoDeDisparo.position, puntoDeDisparo.rotation);

        // 2. Empujarla hacia adelante
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(puntoDeDisparo.forward * fuerzaDisparo);
        }

        // 3. Destruir la bala después de 3 segundos para que no sature la memoria
        Destroy(bala, 3f);

        // --- ESTO ES NUEVO (Subir la temperatura) ---
        calorActual += calorPorDisparo;

        if (calorActual >= calorMaximo)
        {
            estaSobrecalentada = true; // Bloquea el arma
        }
        // --------------------------------------------
    }
}
