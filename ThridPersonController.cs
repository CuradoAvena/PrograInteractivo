using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 700f;


    private CharacterController controller;
    private Animator animator;

    // Variables para suavizar el giro
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // --- CAMBIOS---//

        // 1. Obtenemos hacia dónde mira la cámara
        Vector3 camForward = UnityEngine.Camera.main.transform.forward;
        Vector3 camRight = UnityEngine.Camera.main.transform.right;

        // 2. Aplanamos los vectores (y = 0) para que el personaje no intente volar
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 3. Calculamos la dirección final sumando los vectores
        Vector3 direction = (camForward * vertical + camRight * horizontal).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * moveSpeed * Time.deltaTime);
        }

        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        animator.SetFloat("Speed", direction.magnitude);
    }
}
