using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaController : MonoBehaviour
{
    public static MetaController Instance;
    
    public Transform meta; // Referencia a la meta para moverla

    // Posiciones de la meta para cada nivel
    private Vector2[] posicionesMeta = new Vector2[]
    {
        new Vector2(2.42f, 2.78f),  // Nivell 1
        new Vector2(8.82f, -2.40f), // Nivell 2
        new Vector2(18.47f, -7.41f), // Nivell 3
        new Vector2(15.84f, -5.54f), // Nivell 4
        new Vector2(22.31f, -5.54f)  // Nivell 5
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void CambiarDeNivel()
    {
        string nivelActual = SceneManager.GetActiveScene().name;

        // Cambia al siguiente nivel o a la PantallaFinal si estás en Nivell5
        switch (nivelActual)
        {
            case "Nivell1":
                SceneManager.LoadScene("Nivell2");
                break;
            case "Nivell2":
                SceneManager.LoadScene("Nivell3");
                break;
            case "Nivell3":
                SceneManager.LoadScene("Nivell4");
                break;
            case "Nivell4":
                SceneManager.LoadScene("Nivell5");
                break;
            case "Nivell5":
                SceneManager.LoadScene("PantallaFinal");  // Cambia a PantallaFinal después de Nivell5
                break;
            default:
                Debug.Log("Ya estás en el último nivel o en una escena desconocida.");
                break;
        }
    }

    // Llama a esta función después de que la escena se cargue para mover la meta
    public void AjustarPosicionMeta()
    {
        string nivelActual = SceneManager.GetActiveScene().name;

        switch (nivelActual)
        {
            case "Nivell1":
                meta.position = posicionesMeta[0];
                break;
            case "Nivell2":
                meta.position = posicionesMeta[1];
                break;
            case "Nivell3":
                meta.position = posicionesMeta[2];
                break;
            case "Nivell4":
                meta.position = posicionesMeta[3];
                break;
            case "Nivell5":
                meta.position = posicionesMeta[4];
                break;
            default:
                Debug.Log("Nivel desconocido, no se puede ajustar la posición de la meta.");
                break;
        }
    }
}
