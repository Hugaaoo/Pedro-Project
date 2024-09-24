using UnityEngine;

public class BoxColliderGizmo : MonoBehaviour
{
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
