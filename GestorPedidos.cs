using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GestorPedidos : MonoBehaviour
{
    public static GestorPedidos Instancia { get; private set; }

    public event Action<List<TipoProducto>> AlCambiarPedidos;
    public event Action<int> AlCambiarPuntaje;

    // NUEVOS EVENTOS PARA EL TIEMPO
    public event Action<float> AlCambiarTiempo;
    public event Action AlTerminarJuego;

    [Header("Configuración de Tiempo")]
    public float tiempoEntrePedidos = 5f;
    public int maximoPedidosEnPantalla = 5;

    [Header("Configuración del Nivel")]
    [Tooltip("Tiempo total del nivel en segundos (Ej. 120 = 2 minutos)")]
    public float tiempoDelNivel = 120f;

    private List<TipoProducto> pedidosActivos = new List<TipoProducto>();
    private int puntaje = 0;
    private bool juegoTerminado = false; // Candado para saber si ya perdimos

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);

        // Asegurarnos de que el tiempo corra normal al iniciar
        Time.timeScale = 1f;
    }

    private void Start()
    {
        StartCoroutine(RutinaGenerarPedidos());
    }

    // NUEVO: El Update se ejecuta cada frame y le resta tiempo al reloj
    private void Update()
    {
        if (juegoTerminado) return; // Si ya se acabó, no hacemos nada

        tiempoDelNivel -= Time.deltaTime; // Restamos los milisegundos que pasaron

        // Le avisamos a la pantalla cuánto tiempo queda
        AlCambiarTiempo?.Invoke(tiempoDelNivel);

        // ¿Se acabó el tiempo?
        if (tiempoDelNivel <= 0)
        {
            tiempoDelNivel = 0;
            TerminarTurno();
        }
    }

    private void TerminarTurno()
    {
        juegoTerminado = true;
        Time.timeScale = 0f; // ¡Magia! Esto congela todas las físicas y movimientos en Unity

        AlTerminarJuego?.Invoke(); // Le gritamos a la UI que muestre la pantalla final
    }

    private IEnumerator RutinaGenerarPedidos()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (pedidosActivos.Count < maximoPedidosEnPantalla)
            {
                GenerarPedidoAleatorio();
            }
            yield return new WaitForSeconds(tiempoEntrePedidos);
        }
    }

    private void GenerarPedidoAleatorio()
    {
        Array valores = Enum.GetValues(typeof(TipoProducto));
        TipoProducto nuevoPedido = (TipoProducto)valores.GetValue(UnityEngine.Random.Range(0, valores.Length));

        pedidosActivos.Add(nuevoPedido);
        AlCambiarPedidos?.Invoke(pedidosActivos);
    }

    public bool IntentarEntregar(TipoProducto productoEntregado)
    {
        if (pedidosActivos.Contains(productoEntregado))
        {
            pedidosActivos.Remove(productoEntregado);
            puntaje += 15;

            AlCambiarPedidos?.Invoke(pedidosActivos);
            AlCambiarPuntaje?.Invoke(puntaje);
            return true;
        }
        return false;
    }
}
