using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform respawnPoint; // Le point de r�apparition d�fini par un objet Empty

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifie si l'objet qui entre en collision est le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            // R�cup�re le script du joueur et appelle la fonction de r�apparition
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null && respawnPoint != null)
            {
                playerController.Respawn(respawnPoint.position);
            }
        }
    }
}
