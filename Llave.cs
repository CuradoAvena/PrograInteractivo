using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{
       // Usamos OnTriggerEnter porque el personaje "camina sobre" el objeto
    void OnTriggerEnter(Collider other)
    {
        // Verificamos que sea el Player quien toca la llave
        // IMPORTANTE: Tu personaje debe tener el Tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log(">> SENSOR ACTIVADO: CREDENCIAL RECIBIDA");

            // Comunicación con el Sistema Central (GameManager)
            GameManager.Instance.RecogerLlave();

            // Feedback: Destruir objeto
            Destroy(gameObject);
        }
    }
}

