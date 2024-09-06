using UnityEngine;

public class JumpZone : MonoBehaviour
{
    private bool playerInside = false; // Pour vérifier si le joueur est dans la zone

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 10f); // Ajuster la force du saut ici
        }
    }
}
