using UnityEngine;

public class VentiladorScript : MonoBehaviour
{
    // Parámetros configurables
    public Vector2 direccion = Vector2.right;  // Dirección de la fuerza del ventilador
    public float fuerza = 5f;                  // Fuerza que aplicará el ventilador
    public ParticleSystem aire;                // Referencia al sistema de partículas

    private void Start()
    {
        // Asegúrate de que el sistema de partículas siempre esté activo al inicio
        if (!aire.isPlaying)
        {
            aire.Play(); // Inicia las partículas cuando comience el juego
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Verifica si el objeto que entra es la pelota
        if (other.CompareTag("Ball"))
        {
            // Obtiene el Rigidbody2D de la pelota
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Aplica una fuerza continua en la dirección especificada
                rb.AddForce(direccion.normalized * fuerza);

                // Asegúrate de que las partículas estén siempre activas
                if (!aire.isPlaying)
                {
                    aire.Play(); // Reproduce el sistema de partículas si no está ya reproduciéndose
                }
            }
        }
    }

}
