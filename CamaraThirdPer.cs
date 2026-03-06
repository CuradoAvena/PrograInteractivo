using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraThirdPer : MonoBehaviour
{
    [Header("Objetivo")]
    [Tooltip("Arrastra aquí la cápsula de tu Jugador")]
    public Transform jugador;

    [Header("Configuración de Cámara")]
    public float distancia = 5f; // Qué tan lejos está la cámara
    public float altura = 1.5f;  // A qué altura apunta (hacia la cabeza/hombros)
    public float sensibilidad = 0.2f;

    [Header("Controles (Unity 6 Input System)")]
    [Tooltip("Configura aquí el movimiento del Mouse")]
    public InputAction mirarAction;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void OnEnable()
    {
        mirarAction.Enable();
    }

    void OnDisable()
    {
        mirarAction.Disable();
    }

    void Start()
    {
        // Ocultar y bloquear el cursor en el centro de la pantalla para que no se salga del juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        // 1. Leer el movimiento del mouse (arriba/abajo, izquierda/derecha)
        Vector2 deltaRaton = mirarAction.ReadValue<Vector2>();

        // 2. Acumular esa rotación multiplicada por la sensibilidad
        rotacionX += deltaRaton.x * sensibilidad;
        rotacionY -= deltaRaton.y * sensibilidad;

        // 3. Limitar la cámara para que no dé la vuelta por debajo del piso ni por encima de la cabeza
        rotacionY = Mathf.Clamp(rotacionY, -20f, 60f);

        // 4. Calcular la rotación y posición matemática
        Quaternion rotacionActual = Quaternion.Euler(rotacionY, rotacionX, 0);
        Vector3 posicionObjetivo = jugador.position + Vector3.up * altura;

        // 5. Aplicarle los valores a la cámara
        transform.position = posicionObjetivo - (rotacionActual * Vector3.forward * distancia);
        transform.LookAt(posicionObjetivo);

        // 6. ¡TRUCO DE ORO! Hacer que el jugador gire automáticamente hacia donde mira la cámara
        jugador.rotation = Quaternion.Euler(0, rotacionX, 0);
    }
}
