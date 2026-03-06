using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class JugadorF : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 8f;
    public float fuerzaSalto = 7f;

    [Header("Controles (Unity 6 Input System)")]
    [Tooltip("Configura aquí el WASD o el Joystick")]
    public InputAction moverAction;
    [Tooltip("Configura aquí la barra espaciadora o el botón A")]
    public InputAction saltarAction;

    [Header("Detección de Suelo")]
    public float distanciaAlSuelo = 1.1f;
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private Vector2 movimientoInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // En el nuevo sistema, las acciones DEBEN encenderse y apagarse
    void OnEnable()
    {
        moverAction.Enable();
        saltarAction.Enable();
    }

    void OnDisable()
    {
        moverAction.Disable();
        saltarAction.Disable();
    }

    void Update()
    {
        // 1. Leer el Vector2 directamente de la acción (X e Y)
        movimientoInput = moverAction.ReadValue<Vector2>();

        // 2. WasPressedThisFrame es el nuevo equivalente a GetButtonDown
        if (saltarAction.WasPressedThisFrame() && EstaEnElSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 3. Aplicar el movimiento. Transformamos la X y la Y a los ejes 3D del mundo
        Vector3 direccion = transform.right * movimientoInput.x + transform.forward * movimientoInput.y;
        rb.MovePosition(rb.position + direccion * velocidad * Time.fixedDeltaTime);
    }

    bool EstaEnElSuelo()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanciaAlSuelo, capaSuelo);
    }
}
