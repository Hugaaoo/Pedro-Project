using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public Vector2 offset; // D�calage de la cam�ra par rapport au joueur

    void LateUpdate()
    {
        if (player != null)
        {
            // Suivre la position exacte du joueur, sans Lerp
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }
}
