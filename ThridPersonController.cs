using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float turnSpeed = 700f; // Velocidad de giro

    [Header("Sistema de Recolección")]
    public GameObject puerta; // NUEVO: Aquí arrastraremos la puerta en el Inspector
    public int cantidadParaGanar = 3; // NUEVO: Cuántos objetos necesitas

    private int objetosRecogidos = 0; // NUEVO: Contador interno

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        // Obtenemos las referencias a los componentes automáticamente
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Obtener input del teclado (WASD o Flechas)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Creamos un vector de dirección. 
        // "normalized" evita que moverse en diagonal sea más rápido.
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // 2. Rotación del personaje hacia donde camina
            // Calculamos el ángulo hacia donde queremos mirar
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Suavizamos el giro para que no sea instantáneo
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 3. Mover al personaje
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }

        // 4. Aplicar gravedad simple (para que no flote si hay desniveles)
        // Nota: Si tu juego es totalmente plano, esto es opcional pero recomendado
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        // 5. Actualizar Animación
        // Pasamos la magnitud del movimiento (0 si está quieto, 1 si se mueve) al Animator
        animator.SetFloat("Speed", direction.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si lo que tocamos tiene la etiqueta correcta
        if (other.gameObject.CompareTag("PickUp"))
        {
            // 1. Sumamos 1 al contador
            objetosRecogidos++; // NUEVO
            Debug.Log("Llevas: " + objetosRecogidos);

            // 2. Destruimos el objeto
            Destroy(other.gameObject);

            // 3. Comprobamos si ya tenemos los necesarios
            if (objetosRecogidos >= cantidadParaGanar) // NUEVO
            {
                AbrirLaPuerta();
            }
        }
    }

    void AbrirLaPuerta()
    {
        Debug.Log("¡Puerta Abierta!");

        PuertaMisteriosa scriptPuerta = puerta.GetComponent<PuertaMisteriosa>();

        if (scriptPuerta != null)
        {
            scriptPuerta.AbrirPuerta();
            Debug.Log("Orden de abrir enviada a la puerta");
        }
    }
    // Variables auxiliares para suavizar el giro
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;
}
