using UnityEngine;

public class InfoObject : MonoBehaviour
{
    [Header("Datos del Artefacto")]
    public string nombre = "Artefacto Desconocido";
    [TextArea] public string descripcion = "Sin datos. Requiere análisis.";

    // Color para feedback visual cuando lo miramos
    public Color colorAlMirar = Color.yellow;
    private Color colorOriginal;
    private Renderer miRender;

    void Start()
    {
        miRender = GetComponent<Renderer>();
        colorOriginal = miRender.material.color;
    }

    // La cámara llamará a esto cuando el rayo lo toque
    public void Resaltar(bool activo)
    {
        if (activo)
        {
            miRender.material.color = colorAlMirar;
        }
        else
        {
            miRender.material.color = colorOriginal;
        }
    }
}

   