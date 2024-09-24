using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{



    public float moveSpeed = 5f; // Vitesse de déplacement horizontal
    public float jumpForce = 10f; // Force de saut initial
    public float jumpHoldForce = 5f; // Force appliquée tant que la touche Espace est maintenue
    public float maxJumpTime = 0.5f; // Durée maximale pendant laquelle le saut peut être prolongé
    public Transform respawnPoint; // Point de réapparition
    public float spaceMoveSpeed = 8f; // Vitesse de déplacement en phase de vaisseau
    public float spaceLiftForce = 10f; // Force de montée du vaisseau
    public float gravityScale = 1f; // Échelle de gravité normale du joueur

    // Variables pour Space Invader phase
    public GameObject projectilePrefab; // Préfab pour les tirs
    public Transform firePoint; // Point de départ des tirs
    public Camera mainCamera; // Référence à la caméra principale pour le défilement
    public float sceneScrollSpeed = 2f; // Vitesse de défilement en mode Space Invader

    private Rigidbody2D rb;
    private bool isGrounded; // Pour vérifier si le joueur est au sol
    private float jumpTimeCounter; // Compteur de temps pour la durée du saut
    private bool isSpacePhase = false; // Pour vérifier si le joueur est en mode vaisseau spatial
    private bool isInZone = false; // Pour vérifier si le joueur est dans une zone de changement de gameplay
    private bool isInSpaceInvaderZone = false; // Pour vérifier si le joueur est dans la zone Space Invader

    public float compressionDuration = 0.1f;  // Durée de la compression avant le saut
    public float smoothDuration = 0.2f;       // Durée pour que la transition soit fluide
    public Vector3 compressedScale = new Vector3(1f, 0.8f, 1f); // Échelle compressée
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);       // Échelle normale

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Initialiser la gravité
    }


    void Update()
    {
        if (isInSpaceInvaderZone)
        {
            HandleSpaceInvaderMode(); // Activer les contrôles Space Invader
            ScrollSceneWithCamera(); // Faire défiler la scène avec la caméra
        }
        else if (isInZone)
        {
            HandleZoneControls(); // Contrôles de zone (flèches et saut)
        }
        else if (!isSpacePhase)
        {
            HandleRunningPhase(); // Contrôles normaux
        }
        else
        {
            HandleSpacePhase(); // Contrôles de la phase de vaisseau
        }
    }

    // Fonction pour gérer la phase de course standard
    void HandleRunningPhase()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Appliquer la vitesse horizontale

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            // Lancer la coroutine pour une compression douce
            StartCoroutine(SmoothCompressAndExtend());
        }

        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpHoldForce * Time.deltaTime);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
        }
    }

    IEnumerator SmoothCompressAndExtend()
    {
        // Compression douce
        yield return LerpScale(compressedScale, compressionDuration);

        // Revenir à l'échelle normale doucement pendant le saut
        yield return LerpScale(normalScale, smoothDuration);
    }

    // Fonction pour interpoler l'échelle du joueur en douceur
    IEnumerator LerpScale(Vector3 targetScale, float duration)
    {
        Vector3 currentScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  // Attendre jusqu'à la prochaine frame
        }

        // S'assurer que l'échelle finale est exactement celle désirée
        transform.localScale = targetScale;
    }


    // Fonction pour gérer la phase de vaisseau spatial
    void HandleSpacePhase()
    {
        rb.velocity = new Vector2(spaceMoveSpeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, spaceLiftForce);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -spaceLiftForce); // Applique une force descendante si Espace n'est pas appuyé
        }
    }

    // Fonction pour gérer les contrôles dans la zone : déplacement avec flèches et saut avec Espace
    void HandleZoneControls()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Flèches gauche/droite
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Saut avec Espace
        }
    }

    // Contrôles en mode Space Invader
    void HandleSpaceInvaderMode()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Appliquer les mouvements sans gravité
        rb.velocity = new Vector2(horizontalInput * spaceMoveSpeed, verticalInput * spaceMoveSpeed);

        // Verrouiller la rotation du joueur pour qu'il reste droit
        transform.rotation = Quaternion.identity;

        // Tirer des projectiles avec Espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectile();
        }
    }

    // Fonction pour tirer des projectiles en mode Space Invader
    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // Pas de gravité pour les projectiles, toujours vers l'avant
        projectileRb.gravityScale = 0;
        projectileRb.velocity = Vector2.up * spaceLiftForce; // Les projectiles se déplacent vers le haut
    }

    // Faire défiler la caméra uniquement sur l'axe Y
    void ScrollSceneWithCamera()
    {
        Vector3 cameraTargetPosition = new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPosition, Time.deltaTime * sceneScrollSpeed);
    }

    public void StartSpacePhase()
    {
        isSpacePhase = true;
        rb.gravityScale = 0; // Désactiver la gravité pour la phase de vaisseau spatial
        transform.rotation = Quaternion.identity; // Réinitialiser la rotation
    }

    public void EndSpacePhase()
    {
        isSpacePhase = false;
        rb.gravityScale = gravityScale; // Réactiver la gravité pour la phase normale
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Réinitialiser la vitesse horizontale pour la phase normale
        transform.rotation = Quaternion.identity; // Réinitialiser la rotation
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position; // Réinitialiser la position du joueur
        rb.velocity = Vector2.zero; // Réinitialiser la vélocité du joueur
        rb.angularVelocity = 0f; // Réinitialiser la rotation angulaire (pour les collisions complexes)
        transform.rotation = Quaternion.identity; // Réinitialiser la rotation du joueur
        isGrounded = false; // Le joueur commence en l'air après le respawn

        StartCoroutine(DelayRespawn()); // Appeler la coroutine pour le délai

        if (isSpacePhase)
        {
            EndSpacePhase(); // Sortir de la phase de vaisseau spatial si le joueur respawn
        }
    }

    // Détecter si le joueur entre dans une zone spécifique pour changer le gameplay
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = true; // Activer les contrôles spéciaux
            rb.freezeRotation = true; // Verrouiller la rotation
        }

        if (other.CompareTag("SpaceInvaderZone"))
        {
            isInSpaceInvaderZone = true; // Activer le mode Space Invader
            rb.gravityScale = 0; // Désactiver la gravité pour le joueur
        }
    }

    // Détecter si le joueur sort de la zone pour revenir aux contrôles normaux
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = false; // Désactiver les contrôles spéciaux
            rb.freezeRotation = false; // Réactiver la rotation si nécessaire
        }

        if (other.CompareTag("SpaceInvaderZone"))
        {
            isInSpaceInvaderZone = false; // Désactiver le mode Space Invader
            rb.gravityScale = gravityScale; // Réactiver la gravité pour le joueur
        }
    }

    // Détecter si le joueur est au sol
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true; // Le joueur est au sol

            // Arrêter l'animation de saut
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn(respawnPoint.position); // Réapparaître si le joueur touche un obstacle
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false; // Le joueur quitte le sol
    }

    // Coroutine pour ajouter un délai après le respawn


    // Coroutine pour ajouter un délai après le respawn
    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(0.1f); // Attendre un court instant avant de permettre des interactions
        rb.velocity = Vector2.zero; // Assurer que la vélocité reste à zéro
    }
}
