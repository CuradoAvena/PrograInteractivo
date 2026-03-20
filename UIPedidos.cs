using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPedidos : MonoBehaviour
{
    [Header("Textos de la Interfaz")]
    public TextMeshProUGUI textoListaPedidos;
    public TextMeshProUGUI textoPuntaje;

    // NUEVOS ELEMENTOS DE UI
    public TextMeshProUGUI textoReloj;
    public GameObject panelGameOver;
    public TextMeshProUGUI textoPuntajeFinal;

    private void Start()
    {
        // Nos suscribimos a todos los eventos
        GestorPedidos.Instancia.AlCambiarPedidos += ActualizarPantallaPedidos;
        GestorPedidos.Instancia.AlCambiarPuntaje += ActualizarPuntaje;

        GestorPedidos.Instancia.AlCambiarTiempo += ActualizarReloj;
        GestorPedidos.Instancia.AlTerminarJuego += MostrarGameOver;

        // Asegurarnos de que el panel de Game Over esté apagado al iniciar
        panelGameOver.SetActive(false);

        ActualizarPuntaje(0);
    }

    private void OnDestroy()
    {
        if (GestorPedidos.Instancia != null)
        {
            GestorPedidos.Instancia.AlCambiarPedidos -= ActualizarPantallaPedidos;
            GestorPedidos.Instancia.AlCambiarPuntaje -= ActualizarPuntaje;

            GestorPedidos.Instancia.AlCambiarTiempo -= ActualizarReloj;
            GestorPedidos.Instancia.AlTerminarJuego -= MostrarGameOver;
        }
    }

    // NUEVO: Convierte los segundos (Ej. 115) en formato de reloj (01:55)
    private void ActualizarReloj(float tiempoRestante)
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        textoReloj.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Toque de tensión: Si quedan 10 segundos o menos, el reloj se pone rojo
        if (tiempoRestante <= 10f)
        {
            textoReloj.color = Color.red;
        }
    }

    // NUEVO: Prende el panel final y copia el puntaje
    private void MostrarGameOver()
    {
        panelGameOver.SetActive(true);
        textoPuntajeFinal.text = textoPuntaje.text; // Copiamos los puntos que hizo

        Cursor.visible = true; // Hacemos que la flechita se vuelva a ver
        Cursor.lockState = CursorLockMode.None;
    }

    private void ActualizarPantallaPedidos(List<TipoProducto> pedidosActivos)
    {
        textoListaPedidos.text = "PEDIDOS ACTIVOS:\n\n";
        if (pedidosActivos.Count == 0)
        {
            textoListaPedidos.text += "Esperando clientes...";
            return;
        }
        foreach (TipoProducto pedido in pedidosActivos)
        {
            textoListaPedidos.text += "- Cubo " + pedido.ToString() + "\n";
        }
    }

    private void ActualizarPuntaje(int nuevoPuntaje)
    {
        textoPuntaje.text = "Puntos: " + nuevoPuntaje;
    }

    public void ReiniciarJuego()
    {
        // Le pedimos a Unity que vuelva a cargar la escena en la que estamos ahorita
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}