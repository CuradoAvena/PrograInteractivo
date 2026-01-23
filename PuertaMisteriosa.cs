using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaMisteriosa : MonoBehaviour
{
     public bool estaAbierta = false;
 public float velocidadApertura = 2.0f;
 public float anguloApertura = 90f; // Grados que va a girar

 private Quaternion rotacionInicial;
 private Quaternion rotacionFinal;

 void Start()
 {
     // Guardamos cómo está la puerta al principio
     rotacionInicial = transform.rotation;
     // Calculamos cómo quedará rotada 90 grados en el eje Y (el verde/vertical)
     rotacionFinal = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + anguloApertura, transform.eulerAngles.z);
 }

 void Update()
 {
     // Si la orden de abrirse es verdadera, rotamos suavemente
     if (estaAbierta)
     {
         // Quaternion.Slerp hace una transición suave entre dos rotaciones
         transform.rotation = Quaternion.Slerp(transform.rotation, rotacionFinal, Time.deltaTime * velocidadApertura);
     }
 }

 // Esta es la función que llamará tu Personaje
 public void AbrirPuerta()
 {
     estaAbierta = true;
 }
}

