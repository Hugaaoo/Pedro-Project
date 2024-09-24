using UnityEngine;

public class EmptyParentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de d�placement de l'empty parent
    public Transform player; // R�f�rence au joueur
    public new Transform camera; // R�f�rence � la cam�ra dans l'empty parent
    private bool isInSpaceInvaderZone = false; // Pour v�rifier si le joueur est dans la zone Space Invader

    void Update()
    {
        if (isInSpaceInvaderZone)
        {
            MoveEmptyParent(); // D�placer l'empty parent en Y
        }
    }

    // D�placer l'empty parent en Y
    void MoveEmptyParent()
    {
        // D�placer l'empty parent vers le haut
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    // D�tection d'entr�e dans la zone Space Invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        // V�rifier que c'est bien le joueur qui entre dans la zone
        if (other.gameObject.CompareTag("Player"))
        {
            isInSpaceInvaderZone = true; // Activer le d�placement de l'empty parent
        }
    }

    // D�tection de sortie de la zone Space Invader
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInSpaceInvaderZone = false; // Arr�ter le d�placement de l'empty parent
        }
    }
}
