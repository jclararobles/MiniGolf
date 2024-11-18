using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField nombreInput;      // InputField para el nombre del usuario
    public TextMeshProUGUI rankingText;     // Texto para mostrar el ranking
    public Button jugarButton;              // Botón para comenzar la partida

    private List<int> mejoresResultados;    // Lista de los mejores resultados
    private List<string> mejoresJugadores;  // Lista de los nombres de los mejores jugadores

    private void Start()
    {
        // Configurar el botón para iniciar la partida
        jugarButton.onClick.AddListener(IniciarJuego);

        // Cargar y mostrar el ranking de los 5 mejores resultados
        CargarRanking();
        MostrarRanking();
    }

    private void OnEnable() {
        // Cargar el ranking cada vez que se activa el menú principal
        CargarRanking();
        MostrarRanking();
    }

    private void IniciarJuego()
    {
        // Guarda el nombre ingresado
        PlayerPrefs.SetString("nombreJugador", nombreInput.text);

        // Carga la primera escena del juego (por ejemplo, "Nivell1")
        SceneManager.LoadScene("Nivell1");
    }

    private void CargarRanking()
    {
        mejoresResultados = new List<int>();
        mejoresJugadores = new List<string>();

        // Cargar los 5 mejores resultados y los nombres desde PlayerPrefs
        for (int i = 1; i <= 5; i++)
        {
            int resultado = PlayerPrefs.GetInt("MejorResultado" + i, int.MaxValue);
            string jugador = PlayerPrefs.GetString("MejorJugador" + i, "");

            if (resultado != int.MaxValue && !string.IsNullOrEmpty(jugador))
            {
                mejoresResultados.Add(resultado);
                mejoresJugadores.Add(jugador);
            }
        }

        // Ordenar los resultados de menor a mayor
        for (int i = 0; i < mejoresResultados.Count - 1; i++)
        {
            for (int j = i + 1; j < mejoresResultados.Count; j++)
            {
                if (mejoresResultados[i] > mejoresResultados[j])
                {
                    // Intercambiar resultados
                    int tempResultado = mejoresResultados[i];
                    mejoresResultados[i] = mejoresResultados[j];
                    mejoresResultados[j] = tempResultado;

                    // Intercambiar jugadores
                    string tempJugador = mejoresJugadores[i];
                    mejoresJugadores[i] = mejoresJugadores[j];
                    mejoresJugadores[j] = tempJugador;
                }
            }
        }
    }

    private void MostrarRanking()
    {
        rankingText.text = "Top 5 Mejores Resultados:\n";

        for (int i = 0; i < mejoresResultados.Count; i++)
        {
            rankingText.text += $"{i + 1}. {mejoresJugadores[i]} - {mejoresResultados[i]} golpes\n";
        }
    }

    // Método para actualizar el ranking después de cada partida
    public void ActualizarRanking(int intentosTotales)
    {
        // Añadir el nuevo resultado y el nombre del jugador al ranking
        string jugador = PlayerPrefs.GetString("nombreJugador", "Jugador Desconocido");

        mejoresResultados.Add(intentosTotales);
        mejoresJugadores.Add(jugador);

        // Ordenar los resultados de menor a mayor
        for (int i = 0; i < mejoresResultados.Count - 1; i++)
        {
            for (int j = i + 1; j < mejoresResultados.Count; j++)
            {
                if (mejoresResultados[i] > mejoresResultados[j])
                {
                    // Intercambiar resultados
                    int tempResultado = mejoresResultados[i];
                    mejoresResultados[i] = mejoresResultados[j];
                    mejoresResultados[j] = tempResultado;

                    // Intercambiar jugadores
                    string tempJugador = mejoresJugadores[i];
                    mejoresJugadores[i] = mejoresJugadores[j];
                    mejoresJugadores[j] = tempJugador;
                }
            }
        }

        // Mantener solo los 5 mejores resultados
        if (mejoresResultados.Count > 5)
        {
            mejoresResultados.RemoveAt(5);
            mejoresJugadores.RemoveAt(5);
        }

        // Guardar el ranking en PlayerPrefs
        for (int i = 0; i < mejoresResultados.Count; i++)
        {
            PlayerPrefs.SetInt("MejorResultado" + (i + 1), mejoresResultados[i]);
            PlayerPrefs.SetString("MejorJugador" + (i + 1), mejoresJugadores[i]);
        }

        PlayerPrefs.Save();  // Asegurarse de guardar los cambios
        MostrarRanking();    // Actualizar la visualización del ranking
    }
}
