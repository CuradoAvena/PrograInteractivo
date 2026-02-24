using UnityEngine;

public class Ametralladora : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject balaPrefab;      // La bolita que van a disparar
    public Transform puntoDeDisparo;   // De dónde sale la bala (la punta del cañón)
    public float fuerzaDisparo = 2000f;
    public float cadencia = 0.1f;      // Qué tan rápido dispara (0.1 = muy rápido)

    private float tiempoProximoDisparo = 0f;

    void Update()
    {
        // Si dejan presionado el clic izquierdo y ya pasó el tiempo de recarga
        if (Input.GetButton("Fire1") && Time.time >= tiempoProximoDisparo)
        {
            tiempoProximoDisparo = Time.time + cadencia;
            Disparar();
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
    }
}
