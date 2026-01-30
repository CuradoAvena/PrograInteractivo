using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaMisteriosa : MonoBehaviour
{
    // Hacemos pública esta variable para ver en el Inspector si cambia el check
    public bool estaAbierta = false;

    public float velocidad = 2.0f;
    public float anguloApertura = 90f;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        // Guardamos la rotación inicial como "Cerrada"
        rotacionCerrada = transform.rotation;

        // Calculamos la rotación "Abierta" sumando el ángulo
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + anguloApertura, transform.eulerAngles.z);
    }

    void Update()
    {
        // --- AQUÍ ESTABA EL PROBLEMA ---
        // Antes solo tenías un 'if(estaAbierta)'.
        // Ahora usamos esta lógica: 
        // "¿Estoy abierta? Ve a 'rotacionAbierta'. ¿No? Entonces ve a 'rotacionCerrada'."

        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;

        // Movemos la puerta suavemente hacia el objetivo actual
        transform.rotation = Quaternion.Slerp(transform.rotation, objetivo, Time.deltaTime * velocidad);
    }

    public void AbrirPuerta()
    {
        estaAbierta = true;
    }

    public void CerrarPuerta()
    {
        estaAbierta = false; // Al volverse falso, el Update la mandará de regreso
    }
}
