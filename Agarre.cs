using UnityEngine;
using UnityEngine.InputSystem;

public class Agarre : MonoBehaviour
{
    [Header("Configuracion de las Manos")]
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

   
    private GameObject objetoEnMano;
    private Rigidbody rbObjeto;

    void OnEnable() { interactuarAction.Enable(); }
    void OnDisable() { interactuarAction.Disable(); }

    void Update()
    {
        // 1. Siempre revisamos que estamos mirando para el Feedback Visual
        ManejarFeedbackVisual();

        // 2. Leer el boton para agarrar/soltar
        if (interactuarAction.WasPressedThisFrame())
        {
            if (objetoEnMano == null) IntentarAgarre();
            else SoltarObjeto();
        }
    }

    void FixedUpdate()
    {
        // Expansión C: El Agarre por Físicas (Se hace en FixedUpdate porque son físicas)
        if (objetoEnMano != null && rbObjeto != null)
        {
            // Calculamos la dirección y distancia entre el cubo y nuestras manos
            Vector3 direccionHaciaMano = puntoDeAgarre.position - rbObjeto.position;
            float distancia = direccionHaciaMano.magnitude;

            // Movemos el objeto usando velocidad física, NO teletransportándolo
            rbObjeto.linearVelocity = direccionHaciaMano * fuerzaAtraccion * distancia;
        }
    }

    void ManejarFeedbackVisual()
    {
        // Si ya tenemos algo en la mano, no iluminamos nada más
        if (objetoEnMano != null)
        {
            ApagarBrillo();
            return;
        }

        Ray rayo = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaAgarre, capaInteractuable))
        {
            Renderer rend = impacto.collider.GetComponent<Renderer>();

            // Si miramos a un cubo nuevo que no está iluminado
            if (rend != null && rend != rendererIluminado)
            {
                ApagarBrillo(); // Apagamos el anterior por si acaso

                rendererIluminado = rend;
                materialOriginal = rend.material; // Guardamos su color gris
                rend.material = materialBrillo;   // Le ponemos el material brillante
            }
        }
        else
        {
            // Si miramos a la pared o al vacío, apagamos el brillo
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

                //(Ya no usamos SetParent ni isKinematic)
                rbObjeto.useGravity = false; // Apagamos la gravedad para que flote
                rbObjeto.linearDamping = 10f;         // Le ponemos mucha fricción al aire para que no gire como loco
                rbObjeto.constraints = RigidbodyConstraints.FreezeRotation; // Congelamos su rotación temporalmente
            }
        }
    }

    void SoltarObjeto()
    {
        // Revertimos las físicas a la normalidad para que caiga
        rbObjeto.useGravity = true;
        rbObjeto.linearDamping = 0f;
        rbObjeto.constraints = RigidbodyConstraints.None;

        objetoEnMano = null;
        rbObjeto = null;
    }
}

