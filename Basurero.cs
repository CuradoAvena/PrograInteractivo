using UnityEngine;

public class Basurero : MonoBehaviour
{
    private void OnTriggerEnter(Collider otro)
    {
        // Medida de seguridad: Si el jugador se cae a la basura, lo ignoramos.
        // Asegúrense de que su cápsula del jugador tenga el Tag "Player"
        if (otro.CompareTag("Player") || otro.CompareTag("MainCamera"))
        {
            return;
        }

        // Si es cualquier otra cosa (basura, crudo o servible), lo destruimos
        Destroy(otro.gameObject);
        Debug.Log("¡Objeto incinerado en la basura!");
    }
}
