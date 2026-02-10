using UnityEngine;

public class InputAlquimia : MonoBehaviour
{
    [Header("Configuración")]
    //Especificamos que es una cámara de Unity
    public UnityEngine.Camera camaraPuzzle;

    void Update()
    {
        // Seguridad: Si olvidaste arrastrar la cámara, no hace nada
        if (camaraPuzzle == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            // CORRECCIÓN 2: Usamos la variable explícita
            Ray ray = camaraPuzzle.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Ingrediente ing = hit.collider.GetComponent<Ingrediente>();
                if (ing != null)
                {
                    // Llama a la función "Agregar"
                    ing.Agregar();
                }
            }
        }
    }
}

