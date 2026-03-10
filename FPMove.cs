using UnityEngine;
using UnityEngine.InputSystem;

public class FPMove : MonoBehaviour
{
    public float velocidad = 8f;
    public float fuerzaSalto = 7f;

    [Header("Controles")]
    public InputAction moverAction;
    public InputAction saltarAction;

    [Header("Detección de Suelo")]
    public float distanciaAlSuelo = 1.1f;
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private Vector2 inputMovimiento;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable() { moverAction.Enable(); saltarAction.Enable(); }
    void OnDisable() { moverAction.Disable(); saltarAction.Disable(); }

    void Update()
    {
        inputMovimiento = moverAction.ReadValue<Vector2>();

        if (saltarAction.WasPressedThisFrame() && EstaEnElSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Nos movemos basándonos en hacia dónde está mirando el jugador
        Vector3 direccion = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;
        rb.MovePosition(rb.position + direccion * velocidad * Time.fixedDeltaTime);
    }

    bool EstaEnElSuelo()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanciaAlSuelo, capaSuelo);
    }
}
