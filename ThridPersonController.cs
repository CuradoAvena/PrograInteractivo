using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
public float turnSpeed = 700f;

// BORRAMOS LAS VARIABLES DE PUERTA Y CONTEO
// El personaje no debe saber contar llaves, eso es trabajo del GameManager.

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

    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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

