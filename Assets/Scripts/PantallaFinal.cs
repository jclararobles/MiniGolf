using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PantallaFinal : MonoBehaviour
{
    public TextMeshProUGUI intentosTotalesText;  // Texto donde se mostrarán los intentos totales
    public Button menuButton;  // Botón para volver al menú

    void Start()
    {
        // Mostrar el número total de intentos al llegar a la pantalla final
        intentosTotalesText.text = "Total de Intentos: " + GameManager.Instance.IntentosTotales;
        
        // Asignar la función VolverAlMenu al botón
        menuButton.onClick.AddListener(VolverAlMenu);
    }

    private void VolverAlMenu()
    {
        // Cargar la escena del menú
        SceneManager.LoadScene("Menu");
    }
}
