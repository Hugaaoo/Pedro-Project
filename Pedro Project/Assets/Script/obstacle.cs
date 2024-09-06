using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform respawnPoint; // Le point de réapparition défini par un objet Empty

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet qui entre en collision est le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            // Récupère le script du joueur et appelle la fonction de réapparition
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null && respawnPoint != null)
            {
                playerController.Respawn(respawnPoint.position);
            }
        }
    }
}
