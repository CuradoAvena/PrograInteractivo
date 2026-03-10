using UnityEngine;
using UnityEngine.InputSystem;

public class AgarreFisico : MonoBehaviour
{
    [Header("Configuración de las Manos")]
    [Tooltip("El punto vacío frente a la cámara donde flotará el objeto")]
    public Transform puntoDeAgarre;
    public float distanciaAgarre = 3f;
    public LayerMask capaInteractuable;

    [Header("Controles")]
    [Tooltip("Botón para agarrar/soltar (Ej. clic izquierdo o tecla E)")]
    public InputAction interactuarAction;

    private GameObject objetoEnMano;
    private Rigidbody rbObjeto;

    void OnEnable() { interactuarAction.Enable(); }
    void OnDisable() { interactuarAction.Disable(); }

    void Update()
    {
        // Si el jugador presiona el botón
        if (interactuarAction.WasPressedThisFrame())
        {
            if (objetoEnMano == null) IntentarAgarre(); // Manos vacías = Agarra
            else SoltarObjeto(); // Manos ocupadas = Suelta
        }
    }

    void IntentarAgarre()
    {
        // Disparamos un rayo desde el centro de la cámara hacia adelante
        Ray rayo = new Ray(transform.position, transform.forward);

        // Si el rayo choca con algo de la capa 'capaInteractuable'
        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaAgarre, capaInteractuable))
        {
            // Nos aseguramos de que el objeto tenga físicas
            if (impacto.collider.GetComponent<Rigidbody>() != null)
            {
                objetoEnMano = impacto.collider.gameObject;
                rbObjeto = objetoEnMano.GetComponent<Rigidbody>();

                // 1. Apagamos su gravedad/físicas para que no se caiga
                rbObjeto.isKinematic = true;

                // 2. Lo teletransportamos al punto de agarre y lo emparentamos
                objetoEnMano.transform.position = puntoDeAgarre.position;
                objetoEnMano.transform.rotation = puntoDeAgarre.rotation;
                objetoEnMano.transform.SetParent(puntoDeAgarre);
            }
        }
    }

    void SoltarObjeto()
    {
        // 1. Lo desemparentamos de la cámara
        objetoEnMano.transform.SetParent(null);

        // 2. Volvemos a encender sus físicas para que caiga al piso
        rbObjeto.isKinematic = false;

        objetoEnMano = null;
        rbObjeto = null;
    }
}
