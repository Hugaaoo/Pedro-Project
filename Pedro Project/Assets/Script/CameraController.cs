using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public float followSpeed = 5f; // Vitesse de suivi
    private bool followVerticalOnly = false; // Suivre uniquement l'axe Y quand activ�

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;

            if (followVerticalOnly)
            {
                targetPosition.x = transform.position.x; // Fixer la cam�ra sur l'axe X
            }
            else
            {
                targetPosition.z = transform.position.z; // Garder la profondeur Z
            }

            // D�placement fluide de la cam�ra
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    // D�tecter l'entr�e dans une zone avec un trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            followVerticalOnly = true; // Activer le suivi uniquement sur Y
        }
    }

    // D�tecter la sortie de la zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            followVerticalOnly = false; // R�activer le suivi complet
        }
    }
}
