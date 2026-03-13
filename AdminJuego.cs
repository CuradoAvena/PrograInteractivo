using TMPro;
using UnityEngine;

public class AdministradorJuego : MonoBehaviour
{
    // ¡El Singleton! Esto permite que cualquier otro script lo encuentre al instante
    public static AdministradorJuego instancia;

    [Header("Interfaz Gráfica")]
    public TextMeshProUGUI textoPuntaje;

    private int puntosTotales = 0;

    void Awake()
    {
        // Configuramos al Jefe para que sea único en la escena
        if (instancia == null) { instancia = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // Actualizamos el texto al arrancar el juego
        ActualizarTexto();
    }

    // Cualquier otra máquina puede llamar a esta función pública
    public void SumarPunto()
    {
        puntosTotales++;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        // Cambia la palabra "Entregas" por lo que tus alumnos prefieran
        textoPuntaje.text = "Entregas: " + puntosTotales;
    }
}
