using UnityEngine;

public class SpaceInvaderZone : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de d�placement du joueur
    public float initialMoveUpDistance = 2f; // Distance que le joueur va monter � l'entr�e dans la zone
    public Transform player; // R�f�rence au joueur
    public Camera mainCamera; // R�f�rence � la cam�ra principale
    public float smoothMoveSpeed = 2f; // Vitesse de mont�e fluide

    private bool isInSpaceInvaderZone = false; // Pour v�rifier si le joueur est dans la zone Space Invader
    private bool isMovingUp = false; // V�rifie si le joueur est en train de monter
    private Vector3 targetPosition; // Position cible lors de la mont�e initiale
    private Vector2 screenBounds; // Limites de la cam�ra visible

    void Start()
    {
        // Calculer les limites de la cam�ra en termes de coordonn�es du monde
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

                // V�rifier si le joueur a atteint la position cible
                if (Vector3.Distance(player.position, targetPosition) < 0.01f)
                {
                    isMovingUp = false; // Arr�ter la mont�e une fois la position atteinte
                }
            }
            else
            {
                HandlePlayerMovement(); // G�rer les mouvements dans la zone une fois la mont�e termin�e
            }
        }
    }

    // G�rer les mouvements du joueur dans la zone Space Invader
    void HandlePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculer la nouvelle position en fonction des entr�es du joueur
        Vector3 newPosition = player.position + new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;

        // Contraindre le joueur � rester dans la zone visible de la cam�ra
        newPosition.x = Mathf.Clamp(newPosition.x, mainCamera.transform.position.x - screenBounds.x, mainCamera.transform.position.x + screenBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, mainCamera.transform.position.y - screenBounds.y, mainCamera.transform.position.y + screenBounds.y);

        // Appliquer la nouvelle position au joueur
        player.position = newPosition;

        // Faire suivre la cam�ra uniquement en Y (si n�cessaire, ou la cam�ra peut rester fixe)
        Vector3 cameraNewPosition = new Vector3(mainCamera.transform.position.x, newPosition.y, mainCamera.transform.position.z);
        mainCamera.transform.position = cameraNewPosition;
    }

    // D�tection d'entr�e dans la zone Space Invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpaceInvaderZone = true; // Activer le d�placement dans la zone Space Invader

            // D�finir la position cible pour la mont�e initiale
            targetPosition = new Vector3(player.position.x, player.position.y + initialMoveUpDistance, player.position.z);
            isMovingUp = true; // Activer la mont�e fluide
        }
    }

    // D�tection de sortie de la zone Space Invader
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpaceInvaderZone = false; // D�sactiver le d�placement dans la zone Space Invader
        }
    }

    // D�finir la couleur du Gizmo
    public Color gizmoColor = Color.red;

    // Dessiner le Gizmo dans la sc�ne
    private void OnDrawGizmos()
    {
        // Appliquer la couleur d�finie
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
