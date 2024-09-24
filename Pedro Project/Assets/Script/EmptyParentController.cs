using UnityEngine;

public class EmptyParentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement de l'empty parent
    public Transform player; // Référence au joueur
    public new Transform camera; // Référence à la caméra dans l'empty parent
    private bool isInSpaceInvaderZone = false; // Pour vérifier si le joueur est dans la zone Space Invader

    void Update()
    {
        if (isInSpaceInvaderZone)
        {
            MoveEmptyParent(); // Déplacer l'empty parent en Y
        }
    }

    // Déplacer l'empty parent en Y
    void MoveEmptyParent()
    {
        // Déplacer l'empty parent vers le haut
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    // Détection d'entrée dans la zone Space Invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifier que c'est bien le joueur qui entre dans la zone
        if (other.gameObject.CompareTag("Player"))
        {
            isInSpaceInvaderZone = true; // Activer le déplacement de l'empty parent
        }
    }

    // Détection de sortie de la zone Space Invader
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInSpaceInvaderZone = false; // Arrêter le déplacement de l'empty parent
        }
    }
}
