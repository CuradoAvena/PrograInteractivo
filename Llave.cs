using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{
    void OnMouseDown() // O OnTriggerEnter si usas un First Person Controller
    {
        // COMUNICACIÓN: La llave busca al Instance y reporta
        GameManager.Instance.RecogerLlave();

        // Feedback visual (opcional: sonido o partículas)
        Destroy(gameObject); // La llave desaparece
    }
}
