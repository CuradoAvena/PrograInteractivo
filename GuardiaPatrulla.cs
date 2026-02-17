using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GuardiaPatrulla : MonoBehaviour
{
    [Header("Feedback Visual")]
    public GameObject iconoAlerta;

    [Header("Componentes")]
    public NavMeshAgent agente;
    public Animator animator;
    public Transform jugador; 
    
    [Header("Patrullaje")]
    public Transform[] puntosRuta;
    public float distanciaMinima = 0.5f;
    private int indiceActual = 0;

    [Header("Cono de Visión (Ojos)")]
    public float rangoVision = 10f; // Qué tan lejos ve
    [Range(0, 360)] public float anguloVision = 45f; // Qué tan ancho ve

    [Header("Estado")]
    public bool persiguiendo = false; // Para saber si te vio

    void Start()
    {
        if (agente == null) agente = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        
        if (jugador == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agente.updateRotation = true;
        IrAlSiguientePunto();
    }

    void Update()
    {
        // 1. CHEQUEAR VISIÓN (¿Te veo?)
        if (BuscarJugador())
        {
            persiguiendo = true;

            if (iconoAlerta != null) iconoAlerta.SetActive(true);
        }
        else
        {
           
            if (persiguiendo && Vector3.Distance(transform.position, jugador.position) > rangoVision * 1.5f)
            {
                persiguiendo = false;

                if (iconoAlerta != null) iconoAlerta.SetActive(false);
                IrAlSiguientePunto(); // Vuelve a su ruta
            }
        }

      
        if (persiguiendo)
        {
            // MODO PERSECUCIÓN: Correr hacia el jugador
            agente.destination = jugador.position;
            agente.speed = 4.5f; // Corre más rápido al perseguir
        }
        else
        {
            // MODO PATRULLA: Seguir los puntos
            agente.speed = 2.0f; // Camina tranquilo
            if (!agente.pathPending && agente.remainingDistance < distanciaMinima)
            {
                IrAlSiguientePunto();
            }
        }

        // 3. ANIMACIÓN
        if (animator != null) animator.SetFloat("Speed", agente.velocity.magnitude);
    }

    bool BuscarJugador()
    {
        if (jugador == null) return false;

        // Calculamos la dirección hacia el jugador
        Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // 1. CHEQUEO DE DISTANCIA (¿Está cerca?)
        if (distancia < rangoVision)
        {
            // 2. CHEQUEO DE ÁNGULO (¿Está frente a mis ojos?)
            if (Vector3.Angle(transform.forward, direccionAlJugador) < anguloVision / 2)
            {
                // 3. ¡NUEVO! RAYCAST (¿Hay pared en medio?)
                RaycastHit hit;

                // Lanzamos el rayo desde un poco arriba (Vector3.up) para que salga de la cabeza, no de los pies
                // Así evitamos que choque con el suelo.
                Vector3 origen = transform.position + Vector3.up;

                // Apuntamos al pecho del jugador (jugador + up)
                Vector3 direccionReal = (jugador.position + Vector3.up) - origen;

                if (Physics.Raycast(origen, direccionReal, out hit, rangoVision))
                {
                    // Si lo PRIMERO que toca el rayo es el Jugador... ¡TE VEO!
                    if (hit.transform.CompareTag("Player"))
                    {
                        return true;
                    }
                    else
                    {
                       
                        return false;
                    }
                }
            }
        }
        return false;
    }

    void IrAlSiguientePunto()
    {
        if (puntosRuta.Length == 0) return;
        agente.destination = puntosRuta[indiceActual].position;
        indiceActual = (indiceActual + 1) % puntosRuta.Length;
    }

    // GAME OVER al tocar
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        Vector3 visionDerecha = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;
        Vector3 visionIzquierda = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + visionDerecha * rangoVision);
        Gizmos.DrawLine(transform.position, transform.position + visionIzquierda * rangoVision);
    }
}

