using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonCierre : MonoBehaviour
{
    [Header("Conexión")]
    public PuertaMisteriosa scriptPuerta; // Arrastra la puerta aquí

    [Header("Configuración")]
    public KeyCode teclaAccion = KeyCode.E; // La tecla estándar de interacción

  
    private void OnTriggerStay(Collider other)
    {
        // Validamos que sea el Player
        if (other.CompareTag("Player"))
        {
            // Si presiona la tecla...
            if (Input.GetKeyDown(teclaAccion))
            {
                Debug.Log(">> COMANDO MANUAL: CERRANDO COMPUERTA.");

               
                scriptPuerta.CerrarPuerta();

              
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Debug.Log("Presiona 'E' para cerrar");
    }
}
