using UnityEngine;
using UnityEngine.InputSystem;

public class Agarre : MonoBehaviour
{
    [Header("Configuraci�n de las Manos")]
    public Transform puntoDeAgarre;
    public float distanciaAgarre = 3f;
    public LayerMask capaInteractuable;
    [Tooltip("Fuerza del rayo tractor para mover el objeto")]
    public float fuerzaAtraccion = 15f;

    [Header("Feedback Visual (Expansi�n B)")]
    [Tooltip("El material brillante cuando lo miramos")]
    public Material materialBrillo;
    private Material materialOriginal;
    private Renderer rendererIluminado;

    [Header("Controles")]
    public InputAction interactuarAction;

    // Variables internas
    private GameObject objetoEnMano;
    private Rigidbody rbObjeto;

    void OnEnable() { interactuarAction.Enable(); }
    void OnDisable() { interactuarAction.Disable(); }

    void Update()
    {
        // 1. Siempre revisamos qu� estamos mirando para el Feedback Visual
        ManejarFeedbackVisual();

        // 2. Leer el bot�n para agarrar/soltar
        if (interactuarAction.WasPressedThisFrame())
        {
            if (objetoEnMano == null) IntentarAgarre();
            else SoltarObjeto();
        }
    }

    void FixedUpdate()
    {
        // Expansi�n C: El Agarre por F�sicas (Se hace en FixedUpdate porque son f�sicas)
        if (objetoEnMano != null && rbObjeto != null)
        {
            // Calculamos la direcci�n y distancia entre el cubo y nuestras manos
            Vector3 direccionHaciaMano = puntoDeAgarre.position - rbObjeto.position;
            float distancia = direccionHaciaMano.magnitude;

            // Movemos el objeto usando velocidad f�sica, NO teletransport�ndolo
            rbObjeto.linearVelocity = direccionHaciaMano * fuerzaAtraccion * distancia;
        }
    }

    void ManejarFeedbackVisual()
    {
        // Si ya tenemos algo en la mano, no iluminamos nada m�s
        if (objetoEnMano != null)
        {
            ApagarBrillo();
            return;
        }

        Ray rayo = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaAgarre, capaInteractuable))
        {
            Renderer rend = impacto.collider.GetComponent<Renderer>();

            // Si miramos a un cubo nuevo que no est� iluminado
            if (rend != null && rend != rendererIluminado)
            {
                ApagarBrillo(); // Apagamos el anterior por si acaso

                rendererIluminado = rend;
                materialOriginal = rend.material; // Guardamos su color gris aburrido
                rend.material = materialBrillo;   // Le ponemos el material brillante
            }
        }
        else
        {
            // Si miramos a la pared o al vac�o, apagamos el brillo
            ApagarBrillo();
        }
    }

    void ApagarBrillo()
    {
        if (rendererIluminado != null)
        {
            rendererIluminado.material = materialOriginal; // Le regresamos su material original
            rendererIluminado = null;
        }
    }

    void IntentarAgarre()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaAgarre, capaInteractuable))
        {
            if (impacto.collider.GetComponent<Rigidbody>() != null)
            {
                objetoEnMano = impacto.collider.gameObject;
                rbObjeto = objetoEnMano.GetComponent<Rigidbody>();

                // EXPANSI�N C: Magia de F�sicas (Ya no usamos SetParent ni isKinematic)
                rbObjeto.useGravity = false; // Apagamos la gravedad para que flote
                rbObjeto.linearDamping = 10f;         // Le ponemos mucha fricci�n al aire para que no gire como loco
                rbObjeto.constraints = RigidbodyConstraints.FreezeRotation; // Congelamos su rotaci�n temporalmente
            }
        }
    }

    void SoltarObjeto()
    {
        // Revertimos las f�sicas a la normalidad para que caiga
        rbObjeto.useGravity = true;
        rbObjeto.linearDamping = 0f;
        rbObjeto.constraints = RigidbodyConstraints.None;

        objetoEnMano = null;
        rbObjeto = null;
    }
}
