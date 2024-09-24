using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{



    public float moveSpeed = 5f; // Vitesse de d�placement horizontal
    public float jumpForce = 10f; // Force de saut initial
    public float jumpHoldForce = 5f; // Force appliqu�e tant que la touche Espace est maintenue
    public float maxJumpTime = 0.5f; // Dur�e maximale pendant laquelle le saut peut �tre prolong�
    public Transform respawnPoint; // Point de r�apparition
    public float spaceMoveSpeed = 8f; // Vitesse de d�placement en phase de vaisseau
    public float spaceLiftForce = 10f; // Force de mont�e du vaisseau
    public float gravityScale = 1f; // �chelle de gravit� normale du joueur

    // Variables pour Space Invader phase
    public GameObject projectilePrefab; // Pr�fab pour les tirs
    public Transform firePoint; // Point de d�part des tirs
    public Camera mainCamera; // R�f�rence � la cam�ra principale pour le d�filement
    public float sceneScrollSpeed = 2f; // Vitesse de d�filement en mode Space Invader

    private Rigidbody2D rb;
    private bool isGrounded; // Pour v�rifier si le joueur est au sol
    private float jumpTimeCounter; // Compteur de temps pour la dur�e du saut
    private bool isSpacePhase = false; // Pour v�rifier si le joueur est en mode vaisseau spatial
    private bool isInZone = false; // Pour v�rifier si le joueur est dans une zone de changement de gameplay
    private bool isInSpaceInvaderZone = false; // Pour v�rifier si le joueur est dans la zone Space Invader

    public float compressionDuration = 0.1f;  // Dur�e de la compression avant le saut
    public float smoothDuration = 0.2f;       // Dur�e pour que la transition soit fluide
    public Vector3 compressedScale = new Vector3(1f, 0.8f, 1f); // �chelle compress�e
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);       // �chelle normale

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Initialiser la gravit�
    }


    void Update()
    {
        if (isInSpaceInvaderZone)
        {
            HandleSpaceInvaderMode(); // Activer les contr�les Space Invader
            ScrollSceneWithCamera(); // Faire d�filer la sc�ne avec la cam�ra
        }
        else if (isInZone)
        {
            HandleZoneControls(); // Contr�les de zone (fl�ches et saut)
        }
        else if (!isSpacePhase)
        {
            HandleRunningPhase(); // Contr�les normaux
        }
        else
        {
            HandleSpacePhase(); // Contr�les de la phase de vaisseau
        }
    }

    // Fonction pour g�rer la phase de course standard
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

        // Revenir � l'�chelle normale doucement pendant le saut
        yield return LerpScale(normalScale, smoothDuration);
    }

    // Fonction pour interpoler l'�chelle du joueur en douceur
    IEnumerator LerpScale(Vector3 targetScale, float duration)
    {
        Vector3 currentScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  // Attendre jusqu'� la prochaine frame
        }

        // S'assurer que l'�chelle finale est exactement celle d�sir�e
        transform.localScale = targetScale;
    }


    // Fonction pour g�rer la phase de vaisseau spatial
    void HandleSpacePhase()
    {
        rb.velocity = new Vector2(spaceMoveSpeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, spaceLiftForce);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -spaceLiftForce); // Applique une force descendante si Espace n'est pas appuy�
        }
    }

    // Fonction pour g�rer les contr�les dans la zone : d�placement avec fl�ches et saut avec Espace
    void HandleZoneControls()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Fl�ches gauche/droite
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Saut avec Espace
        }
    }

    // Contr�les en mode Space Invader
    void HandleSpaceInvaderMode()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Appliquer les mouvements sans gravit�
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

        // Pas de gravit� pour les projectiles, toujours vers l'avant
        projectileRb.gravityScale = 0;
        projectileRb.velocity = Vector2.up * spaceLiftForce; // Les projectiles se d�placent vers le haut
    }

    // Faire d�filer la cam�ra uniquement sur l'axe Y
    void ScrollSceneWithCamera()
    {
        Vector3 cameraTargetPosition = new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPosition, Time.deltaTime * sceneScrollSpeed);
    }

    public void StartSpacePhase()
    {
        isSpacePhase = true;
        rb.gravityScale = 0; // D�sactiver la gravit� pour la phase de vaisseau spatial
        transform.rotation = Quaternion.identity; // R�initialiser la rotation
    }

    public void EndSpacePhase()
    {
        isSpacePhase = false;
        rb.gravityScale = gravityScale; // R�activer la gravit� pour la phase normale
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // R�initialiser la vitesse horizontale pour la phase normale
        transform.rotation = Quaternion.identity; // R�initialiser la rotation
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position; // R�initialiser la position du joueur
        rb.velocity = Vector2.zero; // R�initialiser la v�locit� du joueur
        rb.angularVelocity = 0f; // R�initialiser la rotation angulaire (pour les collisions complexes)
        transform.rotation = Quaternion.identity; // R�initialiser la rotation du joueur
        isGrounded = false; // Le joueur commence en l'air apr�s le respawn

        StartCoroutine(DelayRespawn()); // Appeler la coroutine pour le d�lai

        if (isSpacePhase)
        {
            EndSpacePhase(); // Sortir de la phase de vaisseau spatial si le joueur respawn
        }
    }

    // D�tecter si le joueur entre dans une zone sp�cifique pour changer le gameplay
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = true; // Activer les contr�les sp�ciaux
            rb.freezeRotation = true; // Verrouiller la rotation
        }

        if (other.CompareTag("SpaceInvaderZone"))
        {
            isInSpaceInvaderZone = true; // Activer le mode Space Invader
            rb.gravityScale = 0; // D�sactiver la gravit� pour le joueur
        }
    }

    // D�tecter si le joueur sort de la zone pour revenir aux contr�les normaux
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            isInZone = false; // D�sactiver les contr�les sp�ciaux
            rb.freezeRotation = false; // R�activer la rotation si n�cessaire
        }

        if (other.CompareTag("SpaceInvaderZone"))
        {
            isInSpaceInvaderZone = false; // D�sactiver le mode Space Invader
            rb.gravityScale = gravityScale; // R�activer la gravit� pour le joueur
        }
    }

    // D�tecter si le joueur est au sol
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true; // Le joueur est au sol

            // Arr�ter l'animation de saut
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn(respawnPoint.position); // R�appara�tre si le joueur touche un obstacle
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false; // Le joueur quitte le sol
    }

    // Coroutine pour ajouter un d�lai apr�s le respawn


    // Coroutine pour ajouter un d�lai apr�s le respawn
    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(0.1f); // Attendre un court instant avant de permettre des interactions
        rb.velocity = Vector2.zero; // Assurer que la v�locit� reste � z�ro
    }
}
