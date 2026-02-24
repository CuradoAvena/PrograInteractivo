using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Efectos de Explosión")]
    public float radioExplosion = 5f;
    public float fuerzaExplosion = 500f;
    public GameObject particulasExplosion; // Para que le pongan fuego al chocar

    void OnCollisionEnter(Collision collision)
    {
        // Mostrar partículas si asignaron alguna
        if (particulasExplosion != null)
        {
            Instantiate(particulasExplosion, transform.position, transform.rotation);
        }

        // Detectar todo lo que esté cerca del impacto
        Collider[] objetosGolpeados = Physics.OverlapSphere(transform.position, radioExplosion);

        foreach (Collider obj in objetosGolpeados)
        {
            Rigidbody rbObj = obj.GetComponent<Rigidbody>();
            if (rbObj != null)
            {
                // Aplicar la fuerza de explosión a los aviones/cubos
                rbObj.AddExplosionForce(fuerzaExplosion, transform.position, radioExplosion);
            }
        }

        // Destruir la bala al chocar
        Destroy(gameObject);
    }
}
