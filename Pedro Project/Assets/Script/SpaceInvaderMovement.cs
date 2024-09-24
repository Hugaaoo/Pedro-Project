using UnityEngine;

public class SpaceInvaderZone : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement du joueur
    public float initialMoveUpDistance = 2f; // Distance que le joueur va monter à l'entrée dans la zone
    public Transform player; // Référence au joueur
    public Camera mainCamera; // Référence à la caméra principale
    public float smoothMoveSpeed = 2f; // Vitesse de montée fluide

    private bool isInSpaceInvaderZone = false; // Pour vérifier si le joueur est dans la zone Space Invader
    private bool isMovingUp = false; // Vérifie si le joueur est en train de monter
    private Vector3 targetPosition; // Position cible lors de la montée initiale
    private Vector2 screenBounds; // Limites de la caméra visible

    void Start()
    {
        // Calculer les limites de la caméra en termes de coordonnées du monde
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    void Update()
    {
        if (isInSpaceInvaderZone)
        {
            if (isMovingUp)
            {
                // Monter le joueur progressivement vers la position cible
                player.position = Vector3.Lerp(player.position, targetPosition, smoothMoveSpeed * Time.deltaTime);

                // Vérifier si le joueur a atteint la position cible
                if (Vector3.Distance(player.position, targetPosition) < 0.01f)
                {
                    isMovingUp = false; // Arrêter la montée une fois la position atteinte
                }
            }
            else
            {
                HandlePlayerMovement(); // Gérer les mouvements dans la zone une fois la montée terminée
            }
        }
    }

    // Gérer les mouvements du joueur dans la zone Space Invader
    void HandlePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculer la nouvelle position en fonction des entrées du joueur
        Vector3 newPosition = player.position + new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;

        // Contraindre le joueur à rester dans la zone visible de la caméra
        newPosition.x = Mathf.Clamp(newPosition.x, mainCamera.transform.position.x - screenBounds.x, mainCamera.transform.position.x + screenBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, mainCamera.transform.position.y - screenBounds.y, mainCamera.transform.position.y + screenBounds.y);

        // Appliquer la nouvelle position au joueur
        player.position = newPosition;

        // Faire suivre la caméra uniquement en Y (si nécessaire, ou la caméra peut rester fixe)
        Vector3 cameraNewPosition = new Vector3(mainCamera.transform.position.x, newPosition.y, mainCamera.transform.position.z);
        mainCamera.transform.position = cameraNewPosition;
    }

    // Détection d'entrée dans la zone Space Invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpaceInvaderZone = true; // Activer le déplacement dans la zone Space Invader

            // Définir la position cible pour la montée initiale
            targetPosition = new Vector3(player.position.x, player.position.y + initialMoveUpDistance, player.position.z);
            isMovingUp = true; // Activer la montée fluide
        }
    }

    // Détection de sortie de la zone Space Invader
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpaceInvaderZone = false; // Désactiver le déplacement dans la zone Space Invader
        }
    }

    // Définir la couleur du Gizmo
    public Color gizmoColor = Color.red;

    // Dessiner le Gizmo dans la scène
    private void OnDrawGizmos()
    {
        // Appliquer la couleur définie
        Gizmos.color = gizmoColor;

        // Obtenir la taille et la position du BoxCollider2D
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            // Dessiner un rectangle autour du BoxCollider2D
            Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        }
    }
}
