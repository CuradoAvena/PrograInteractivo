using UnityEngine;

public class MesaEntrega : MonoBehaviour
{
    private void OnTriggerEnter(Collider otro)
    {
        // Solo aceptamos el producto final (el cubo servible)
        if (otro.CompareTag("Servible"))
        {
            // 1. Le gritamos al Jefe (El Singleton) que sume un punto
            AdministradorJuego.instancia.SumarPunto();

            // 2. Destruimos el cubo rojo porque ya lo entregamos
            Destroy(otro.gameObject);
        }
    }
}
