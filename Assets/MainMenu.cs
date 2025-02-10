using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject confirmationPanel; // Referencia al panel de confirmación

    // Método para cargar la escena del juego
    public void Play()
    {
        SceneManager.LoadScene("Juego"); // Cambia "Juego" por el nombre de tu escena
    }

    // Método para mostrar el panel de confirmación
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true); // Muestra el panel de confirmación
    }

    // Método para salir del juego
    public void Quit()
    {
        Debug.Log("Saliendo del juego...");

        // Cierra la aplicación
        Application.Quit();

        // Si estás en el editor de Unity, detén la reproducción
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Método para ocultar el panel de confirmación
    public void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false); // Oculta el panel de confirmación
    }
}