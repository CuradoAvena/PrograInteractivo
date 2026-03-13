using UnityEngine;
using System.Collections;
public class MesaProcesamiento : MonoBehaviour
{
    [Header("Fase 1: Procesamiento")]
    public GameObject prefabResultado; // El cubo rojo bueno
    public float tiempoProceso = 3f;

    [Header("Fase 2: Sobrecalentamiento")]
    public GameObject prefabInservible; // El cubo negro / carbón / basura
    public float tiempoParaArruinarse = 5f; // Cuánto tiempo tienen para quitarlo

    private bool estaOcupada = false;
    private GameObject productoActual; // Rastrea el cubo rojo mientras está en la mesa

    private void OnTriggerEnter(Collider otro)
    {
        // Si entra un cubo crudo y la mesa está libre
        if (!estaOcupada && otro.CompareTag("Crudo"))
        {
            StartCoroutine(ProcesarYQuemar(otro.gameObject));
        }
    }

    private void OnTriggerExit(Collider otro)
    {
        // Si el jugador agarra el producto terminado y lo saca del área de la mesa
        if (productoActual != null && otro.gameObject == productoActual)
        {
            productoActual = null; // ¡Lo salvaron! Desvinculamos el objeto
            estaOcupada = false;   // La mesa queda libre para el siguiente crudo
        }
    }

    IEnumerator ProcesarYQuemar(GameObject materialCrudo)
    {
        estaOcupada = true;

        // --- FASE 1: PROCESANDO ---
        Rigidbody rbCrudo = materialCrudo.GetComponent<Rigidbody>();
        if (rbCrudo != null) rbCrudo.isKinematic = true;

        yield return new WaitForSeconds(tiempoProceso);

        // Destruimos el crudo y creamos el producto bueno
        Vector3 posAparicion = transform.position + Vector3.up * 1.5f;
        Destroy(materialCrudo);
        productoActual = Instantiate(prefabResultado, posAparicion, Quaternion.identity);

        // --- FASE 2: CUENTA REGRESIVA DE RUINA ---
        // Usamos un contador que avanza frame por frame para poder interrumpirlo
        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoParaArruinarse)
        {
            // Si 'productoActual' es nulo, significa que OnTriggerExit ya se ejecutó (el jugador se lo llevó)
            if (productoActual == null)
            {
                yield break; // Cortamos la corrutina de tajo. ¡Final feliz!
            }

            tiempoPasado += Time.deltaTime;
            yield return null; // Esperamos al siguiente frame
        }

        // --- FASE 3: SE QUEMÓ ---
        // Si el ciclo terminó y el producto sigue en la mesa...
        if (productoActual != null)
        {
            Vector3 posFinal = productoActual.transform.position;
            Destroy(productoActual);
            Instantiate(prefabInservible, posFinal, Quaternion.identity);

            productoActual = null;
            estaOcupada = false; // Liberamos la mesa, aunque ahora está sucia con basura
        }
    }
}