using UnityEngine;

public class PortalScript : MonoBehaviour
{
    // Referencia al portal de destino específico para este portal
    public Transform portalDestino;
    private float temporizadorCooldown = 0f;
    private bool puedeTeletransportar = true;

    private void Update()
    {
        // Solo permite que el portal se reactive tras un breve intervalo
        if (!puedeTeletransportar)
        {
            temporizadorCooldown += Time.deltaTime;

            // Reactiva el portal cuando se cumple el tiempo de cooldown
            if (temporizadorCooldown >= 2f)
            {
                EstablecerPermiso(true); // Reactiva el portal actual
                if (portalDestino != null)
                {
                    portalDestino.GetComponent<PortalScript>().EstablecerPermiso(true); // Reactiva el portal destino
                }
                temporizadorCooldown = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D objeto)
    {
        // Comprobación de si el objeto es la pelota y si tiene permiso de teletransportar
        if (puedeTeletransportar && objeto.CompareTag("Ball"))
        {
            Rigidbody2D rbObjeto = objeto.GetComponent<Rigidbody2D>();

            if (rbObjeto != null && portalDestino != null)
            {
                Teletransportar(rbObjeto, objeto.transform);
            }
        }
    }

    private void Teletransportar(Rigidbody2D rbObjeto, Transform objeto)
    {
        // Desactiva ambos portales para evitar teletransportaciones múltiples
        EstablecerPermiso(false);
        if (portalDestino != null)
        {
            portalDestino.GetComponent<PortalScript>().EstablecerPermiso(false);
        }

        // Cambia la posición del objeto al portal destino
        objeto.position = portalDestino.position;

        // Mantiene la dirección y la velocidad de la pelota
        Vector2 direccionActual = rbObjeto.velocity.normalized;
        rbObjeto.velocity = direccionActual * rbObjeto.velocity.magnitude;
    }

    // Método para habilitar o deshabilitar el permiso de teletransportación
    public void EstablecerPermiso(bool permiso)
    {
        puedeTeletransportar = permiso;
        temporizadorCooldown = 0;
    }
}
