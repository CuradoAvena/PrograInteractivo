using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // SINGLETON: La referencia global
    public static GameManager Instance;

    public int llavesRecogidas = 0;
    public int totalLlaves = 3;

    // Referencia a la puerta (arrastrada desde el Inspector)
    public PuertaMisteriosa scriptPuerta;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Método que llaman las llaves al ser recogidas
    public void RecogerLlave()
    {
        llavesRecogidas++;
        Debug.Log("Llaves: " + llavesRecogidas + "/" + totalLlaves);

        // LÓGICA DE JUEGO
        if (llavesRecogidas >= totalLlaves)
        {
            Debug.Log("¡SE ABRE EL CAMINO!");
            scriptPuerta.AbrirPuerta(); // Ordenamos abrir la puerta
        }
    }
}
