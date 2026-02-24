using UnityEngine;

public class ObjetoDestruible : MonoBehaviour
{
    [Header("El modelo roto")]
    [Tooltip("Arrastra aquí el Prefab del objeto cortado en pedazos")]
    public GameObject versionRotaPrefab;

    [Header("Efectos Visuales")]
    public GameObject particulasExplosion; // Fuego o humo opcional

    [Header("Limpieza")]
    [Tooltip("Cuántos segundos tardan los pedazos en desaparecer")]
    public float tiempoDeVidaPedazos = 5f;
    // Esta función se activa en el instante que algo choca contra este objeto
    void OnCollisionEnter(Collision collision)
    {
        // Verificamos si lo que nos golpeó tiene la etiqueta "Bala"
        if (collision.gameObject.CompareTag("Bala"))
        {
            RomperEnPedazos();
        }
    }

    void RomperEnPedazos()
    {
        // 1. Mostrar las partículas de humo/fuego si asignaste alguna
        if (particulasExplosion != null)
        {
            Instantiate(particulasExplosion, transform.position, transform.rotation);
        }

        // 2. Instanciar la versión hecha pedazos exactamente en la misma posición y rotación
        if (versionRotaPrefab != null)
        {
            GameObject pedazos = Instantiate(versionRotaPrefab, transform.position, transform.rotation);

            // Buscamos todos los Rigidbodies dentro del modelo fracturado
            Rigidbody[] rbs = pedazos.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(300f, transform.position, 5f);
            }

            Destroy(pedazos, tiempoDeVidaPedazos);
        }

        // 3. Destruir el objeto original (el intacto)
        Destroy(gameObject);
    }
}

