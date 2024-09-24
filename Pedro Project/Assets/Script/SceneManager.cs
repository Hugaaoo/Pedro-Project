using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        // V�rifie si la touche 5 est press�e
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Utilise KeyCode.Alpha5 pour la touche 5 du clavier principal
        {
            RestartCurrentScene(); // Relancer la sc�ne
        }
    }

    // Fonction pour relancer la sc�ne actuelle
    void RestartCurrentScene()
    {
        // R�cup�re l'index de la sc�ne actuelle et la relance
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
