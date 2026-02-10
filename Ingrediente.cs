using UnityEngine;

public class Ingrediente : MonoBehaviour
{
    [Header("Identidad")]
    public string nombreIngrediente = "Rojo"; // Cambiar a "Verde" o "Azul" en el inspector

    [Header("Referencias")]
    public SistemaCaldero caldero; // Arrastrar aquí el caldero

    // Esta función se llama al hacer Click (desde el script de Input)
    public void Agregar()
    {
        // Animación simple: Pequeño salto
        transform.position += Vector3.up * 0.5f;
        Invoke("Bajar", 0.2f);

        // Enviamos el ingrediente al sistema
        caldero.RecibirIngrediente(nombreIngrediente);
    }

    void Bajar()
    {
        transform.position -= Vector3.up * 0.5f;
    }
}
