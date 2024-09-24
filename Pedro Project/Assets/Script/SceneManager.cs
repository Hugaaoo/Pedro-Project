using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        // Vérifie si la touche 5 est pressée
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Utilise KeyCode.Alpha5 pour la touche 5 du clavier principal
        {
            RestartCurrentScene(); // Relancer la scène
        }
    }

    // Fonction pour relancer la scène actuelle
    void RestartCurrentScene()
    {
        // Récupère l'index de la scène actuelle et la relance
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
