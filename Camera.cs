using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target; // El personaje a seguir

    [Header("Ajustes")]
    public float distancia = 5.0f;
    public float sensibilidad = 2.0f;
    public Vector2 limitesVerticales = new Vector2(-20, 45); // Ángulo min y max

    // Variables internas (Matemáticas)
    private float rotacionX = 0.0f;
    private float rotacionY = 0.0f;

    void Start()
    {
        // Ocultar el cursor para que no estorbe (Como en Fortnite/GTA)
        Cursor.lockState = CursorLockMode.Locked;

        // Inicializamos los ángulos con la rotación actual para que no salte
        Vector3 rotacionInicial = transform.eulerAngles;
        rotacionX = rotacionInicial.y;
        rotacionY = rotacionInicial.x;
    }

    void LateUpdate()
    {
        // Seguridad: Si no hay personaje asignado, no hacemos nada
        if (target == null) return;

        // 1. INPUT: Leemos el mouse
        rotacionX += Input.GetAxis("Mouse X") * sensibilidad;
        rotacionY -= Input.GetAxis("Mouse Y") * sensibilidad;

        // 2. CLAMP: Limitamos mirar arriba/abajo
        rotacionY = Mathf.Clamp(rotacionY, limitesVerticales.x, limitesVerticales.y);

        // 3. CÁLCULO: Convertimos los números en una Rotación 3D (Quaternion)
        Quaternion rotacionFinal = Quaternion.Euler(rotacionY, rotacionX, 0);

        // 4. POSICIÓN: (Rotación * Distancia hacia atrás) + Posición del Personaje
        Vector3 posicionFinal = target.position - (rotacionFinal * Vector3.forward * distancia);

        // 5. APLICAR
        transform.position = posicionFinal;
        transform.rotation = rotacionFinal;
    }
}
