using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaCaldero : MonoBehaviour
{
    [Header("Configuración")]
    public string[] recetaCorrecta;
    public GameObject objetoPremio;

    [Header("Conexión Vital")]
    public ZonaAlquimia zonaScript;

    [Header("Feedback")]
    public ParticleSystem particulasExito;
    public ParticleSystem particulasError;

    private List<string> mezclaActual = new List<string>();

    void Start()
    {
        mezclaActual.Clear();
        if (objetoPremio != null) objetoPremio.SetActive(false);
    }

    public void RecibirIngrediente(string ingrediente)
    {
        mezclaActual.Add(ingrediente);
        int indiceActual = mezclaActual.Count - 1;

      
        if (indiceActual >= recetaCorrecta.Length || mezclaActual[indiceActual] != recetaCorrecta[indiceActual])
        {
            Debug.Log(" Error: '" + ingrediente + "' no es el correcto. Reiniciando...");

            // DISPARAR HUMO
            if (particulasError != null) particulasError.Play();

            // BORRAR MEMORIA
            mezclaActual.Clear();

            Debug.Log("¡CALDERO VACÍO! Ya puedes poner el Ingrediente #1.");
            return;
        }

        // 2. INGREDIENTE CORRECTO
        Debug.Log("Ingrediente " + (indiceActual + 1) + " aceptado: " + ingrediente);

        // 3. CHECK DE VICTORIA
        if (mezclaActual.Count == recetaCorrecta.Length)
        {
            Debug.Log("¡SECUENCIA COMPLETADA!");

            if (objetoPremio != null) objetoPremio.SetActive(true);

            // DISPARAR HUMO
            if (particulasExito != null) particulasExito.Play();

            if (zonaScript != null) zonaScript.SalirModoPuzzle();
        }
    }

}
