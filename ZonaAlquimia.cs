using UnityEngine;

public class ZonaAlquimia : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject camaraJugador;
    public GameObject camaraPuzzle;

    [Header("Control Quirúrgico del Jugador")]
    public MonoBehaviour scriptMovimiento;
    public CharacterController controlFisico;

    [Header("Puzzle")]
    public InputAlquimia scriptInput;

    // ESTADO INTERNO
    private bool enModoPuzzle = false;
    private bool puzzleTerminado = false;

    void Start()
    {
        camaraJugador.SetActive(true);
        camaraPuzzle.SetActive(false);
        if (scriptInput != null) scriptInput.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // CONDICIÓN ACTUALIZADA: Solo entramos si NO hemos terminado el puzzle
        if (other.CompareTag("Player") && !enModoPuzzle && !puzzleTerminado)
        {
            ActivarModoPuzzle();
        }
    }

    void ActivarModoPuzzle()
    {
        enModoPuzzle = true;

        camaraJugador.SetActive(false);
        camaraPuzzle.SetActive(true);

        if (scriptMovimiento != null) scriptMovimiento.enabled = false;
        if (controlFisico != null) controlFisico.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (scriptInput != null) scriptInput.enabled = true;
    }

    public void SalirModoPuzzle()
    {
        // 1. MARCAMOS EL PUZZLE COMO TERMINADO PARA QUE NO SE REPITA
        puzzleTerminado = true;
        enModoPuzzle = false;

        // 2. CAMBIO DE CÁMARAS
        camaraPuzzle.SetActive(false);
        camaraJugador.SetActive(true);

        // 3. DESCONGELAR JUGADOR
        if (controlFisico != null) controlFisico.enabled = true;
        if (scriptMovimiento != null) scriptMovimiento.enabled = true;

        // 4. OCULTAR MOUSE
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 5. APAGAR PUZZLE
        if (scriptInput != null) scriptInput.enabled = false;

        Debug.Log("Saliendo del puzzle... Jugador libre.");
    }
}

