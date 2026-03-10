using UnityEngine;
using UnityEngine.InputSystem;

public class FPViiew : MonoBehaviour
{
    public float sensibilidad = 0.2f;
    public Transform cuerpoJugador; // Arrastrar la cápsula aquí en el Inspector

    [Header("Controles")]
    public InputAction mirarAction;

    private float rotacionX = 0f;

    void OnEnable() { mirarAction.Enable(); }
    void OnDisable() { mirarAction.Disable(); }

    void Start()
    {
        // Ocultar y bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 deltaRaton = mirarAction.ReadValue<Vector2>();

        // 1. Mirar arriba y abajo (Rotamos solo la cámara)
        rotacionX -= deltaRaton.y * sensibilidad;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f); // Tope para no rompernos el cuello
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // 2. Mirar a los lados (Rotamos TODO el cuerpo de la cápsula)
        cuerpoJugador.Rotate(Vector3.up * deltaRaton.x * sensibilidad);
    }
}
