using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int intentosTotales = 0;
    private int nivelActual = 1;

    public int IntentosTotales
    {
        get { return intentosTotales; }
        private set { intentosTotales = value; }
    }

    public List<int> MejoresResultados { get; private set; } = new List<int>();  // Lista pública para almacenar los mejores resultados

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);  // Evitar duplicados
        }
    }

    private void Start()
    {
        // Cargar los mejores resultados cuando el juego comienza
        CargarMejoresResultados();
    }

    // Método para agregar intentos
    public void AgregarIntentos(int intentos)
    {
        intentosTotales += intentos;
    }

    // Método para cambiar de nivel
    public void CambiarDeNivel()
    {
        if (nivelActual == 5)
        {
            Debug.Log("Has llegado a la PantallaFinal");
            GuardarResultado(intentosTotales);
            SceneManager.LoadScene(5);  // Cargar PantallaFinal (índice 5 en Build Settings)
        }
        else if (nivelActual > 5)
        {
            Debug.Log("¡Juego completado! No hay más niveles.");
            nivelActual = 1;
            SceneManager.LoadScene("Nivell1");
        }
        else
        {
            nivelActual++;
            SceneManager.LoadScene("Nivell" + nivelActual);  // Cargar el siguiente nivel
        }
    }

    // Método para guardar el resultado al finalizar el juego
    private void GuardarResultado(int intentos)
    {
        List<int> mejoresResultados = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            int resultado = PlayerPrefs.GetInt("MejorResultado" + i, int.MaxValue);
            if (resultado != int.MaxValue)
            {
                mejoresResultados.Add(resultado);
            }
        }

        mejoresResultados.Add(intentos);
        mejoresResultados.Sort();

        if (mejoresResultados.Count > 5)
        {
            mejoresResultados.RemoveAt(5);
        }

        // Guardar los mejores resultados en PlayerPrefs
        for (int i = 0; i < mejoresResultados.Count; i++)
        {
            PlayerPrefs.SetInt("MejorResultado" + (i + 1), mejoresResultados[i]);
        }

        PlayerPrefs.Save();  // Asegurarse de guardar los cambios
        CargarMejoresResultados();  // Actualizar la lista de mejores resultados después de guardar
    }

    // Método para cargar los mejores resultados
    private void CargarMejoresResultados()
    {
        MejoresResultados.Clear();  // Limpiar la lista antes de cargar nuevos resultados
        for (int i = 1; i <= 5; i++)
        {
            int resultado = PlayerPrefs.GetInt("MejorResultado" + i, int.MaxValue);
            if (resultado != int.MaxValue)
            {
                MejoresResultados.Add(resultado);
            }
        }
    }

    // Método para reiniciar el total de intentos
    public void ReiniciarIntentos()
    {
        intentosTotales = 0;
    }
}
