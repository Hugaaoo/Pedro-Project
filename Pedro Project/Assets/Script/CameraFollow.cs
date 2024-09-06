using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public Vector2 offset; // Décalage de la caméra par rapport au joueur

    void LateUpdate()
    {
        if (player != null)
        {
            // Suivre la position exacte du joueur, sans Lerp
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }
}
