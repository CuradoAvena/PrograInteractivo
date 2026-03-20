using UnityEngine;
using System.Collections;
public class Dispensador : MonoBehaviour
{
    [Header("Configuración del Dispensador")]
    [Tooltip("El Prefab del cubo CRUDO que va a generar esta caja")]
    public GameObject prefabCrudo;

    [Tooltip("Tiempo que tarda en aparecer un cubo nuevo después de llevarse el anterior")]
    public float tiempoRecarga = 1f;

    // Llevamos el rastro del cubo que está actualmente esperando a ser recogido
    private GameObject ingredienteActual;

    void Start()
    {
        // Al arrancar el juego, generamos el primer cubo
        GenerarIngrediente();
    }

    private void OnTriggerExit(Collider otro)
    {
        // Si el objeto que acaba de salir de la caja es exactamente el que generamos...
        if (ingredienteActual != null && otro.gameObject == ingredienteActual)
        {
            // Nos desvinculamos de él (porque el jugador ya se lo llevó)
            ingredienteActual = null;

            // Arrancamos el microondas para sacar uno nuevo
            StartCoroutine(Recargar());
        }
    }

    IEnumerator Recargar()
    {
        // Una pequeña pausa para que no aparezca de golpe en la cara del jugador
        yield return new WaitForSeconds(tiempoRecarga);
        GenerarIngrediente();
    }

    void GenerarIngrediente()
    {
        // Instanciamos el prefab ligeramente arriba del centro del dispensador
        Vector3 posicionAparicion = transform.position + (Vector3.up * 0.5f);

        ingredienteActual = Instantiate(prefabCrudo, posicionAparicion, Quaternion.identity);
    }
}
