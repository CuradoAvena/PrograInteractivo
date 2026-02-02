using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [Header("Configuración General")]
    public float distancia = 10f;
    public LayerMask capaEscanear;

    [Header("Restricciones de Alineación")]
    public Transform cuerpoPersonaje; // <--- NUEVO: Aquí arrastras a tu "Y Bot"
    [Range(10, 180)]
    public float anguloMaximo = 60f;  // Grados permitidos (60 es un buen cono de visión)

    [Header("Interfaz (UI)")]
    public Image imagenMira;
    public GameObject panelDatos;
    public TextMeshProUGUI txtNombre;
    public TextMeshProUGUI txtDescripcion;

    private InfoObject ultimoObjeto;

    void Start()
    {
        if (imagenMira != null) imagenMira.enabled = false;
        LimpiarUI();
    }

    void Update()
    {
        // Solo verificamos si el personaje existe
        if (cuerpoPersonaje == null) return;

        bool activandoEscaner = Input.GetKey(KeyCode.LeftShift);

        if (imagenMira != null) imagenMira.enabled = activandoEscaner;

        if (activandoEscaner)
        {
            EscanearEntorno();
        }
        else
        {
            LimpiarUI();
        }
    }

    void EscanearEntorno()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Dibujamos el rayo verde (Cámara)
        Debug.DrawRay(transform.position, transform.forward * distancia, Color.green);

        if (Physics.Raycast(rayo, out hit, distancia, capaEscanear))
        {
            // --- NUEVA LÓGICA DE ÁNGULO ---

            // 1. Calculamos la dirección hacia el objeto desde el pecho del personaje
            Vector3 direccionHaciaObjeto = (hit.point - cuerpoPersonaje.position).normalized;

            // 2. Calculamos el ángulo entre "Hacia donde mira el pecho" y "Donde está el objeto"
            // (Ignoramos la altura Y para que no falle si el objeto está en el suelo)
            Vector3 frentePersonaje = cuerpoPersonaje.forward;
            frentePersonaje.y = 0;
            direccionHaciaObjeto.y = 0;

            float angulo = Vector3.Angle(frentePersonaje, direccionHaciaObjeto);

            // 3. Validación: Si el ángulo es mayor al permitido, el personaje NO está mirando
            if (angulo > anguloMaximo)
            {
                // Opcional: Podrías poner un Debug para ver el ángulo
                // Debug.Log("Ángulo incorrecto: " + angulo);
                LimpiarUI();
                return; // Cortamos aquí. No leemos datos.
            }
            // -----------------------------

            InfoObject datos = hit.collider.GetComponent<InfoObject>();

            if (datos != null)
            {
                if (ultimoObjeto != datos)
                {
                    LimpiarUI();
                    ultimoObjeto = datos;
                    ultimoObjeto.Resaltar(true);
                    MostrarDatos(datos);
                }
            }
        }
        else
        {
            LimpiarUI();
        }
    }

    void MostrarDatos(InfoObject info)
    {
        panelDatos.SetActive(true);
        txtNombre.text = info.nombre;
        txtDescripcion.text = info.descripcion;
    }

    void LimpiarUI()
    {
        if (ultimoObjeto != null)
        {
            ultimoObjeto.Resaltar(false);
            ultimoObjeto = null;
        }
        if (panelDatos != null) panelDatos.SetActive(false);
    }
}
