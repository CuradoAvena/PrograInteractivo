using UnityEngine;

public class ApuntarConMouse : MonoBehaviour
{
    [Header("Límites de Rotación")]
    [Tooltip("El ángulo mínimo hacia la izquierda/derecha")]
    public float anguloMinimo = 0f;

    [Tooltip("El ángulo máximo hacia la izquierda/derecha")]
    public float anguloMaximo = 180f;

    void Update()
    {
        // 1. Trazar el rayo hacia el mouse
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane planoDelSuelo = new Plane(Vector3.up, transform.position);
        float distancia;

        if (planoDelSuelo.Raycast(rayo, out distancia))
        {
            Vector3 puntoParaMirar = rayo.GetPoint(distancia);

            // 2. Calcular la dirección hacia el mouse (ignorando la altura para que no cabecee)
            Vector3 direccion = puntoParaMirar - transform.position;
            direccion.y = 0;

            // 3. Crear la rotación ideal
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            Vector3 angulos = rotacionDeseada.eulerAngles;

            // 4. Truco para que Unity no se confunda con los ángulos mayores a 180
            float anguloY = angulos.y;
            if (anguloY > 180)
            {
                anguloY -= 360;
            }

            // 5. ¡AQUÍ ESTÁ LA MAGIA! Limitamos el ángulo a lo que tú decidas
            anguloY = Mathf.Clamp(anguloY, anguloMinimo, anguloMaximo);

            // 6. Aplicamos la rotación ya limitada
            transform.rotation = Quaternion.Euler(0, anguloY, 0);
        }
    }
}
