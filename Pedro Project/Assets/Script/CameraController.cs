using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float followSpeed = 5f; // Vitesse de suivi
    private bool followVerticalOnly = false; // Suivre uniquement l'axe Y quand activé

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;

            if (followVerticalOnly)
            {
                targetPosition.x = transform.position.x; // Fixer la caméra sur l'axe X
            }
            else
            {
                targetPosition.z = transform.position.z; // Garder la profondeur Z
            }

            // Déplacement fluide de la caméra
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    // Détecter l'entrée dans une zone avec un trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            followVerticalOnly = true; // Activer le suivi uniquement sur Y
        }
    }

    // Détecter la sortie de la zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            followVerticalOnly = false; // Réactiver le suivi complet
        }
    }
}
