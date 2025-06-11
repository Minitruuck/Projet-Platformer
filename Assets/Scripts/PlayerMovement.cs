using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] -> Modifier la valeur de speed depuis Unity


    // Vitesse de déplacement du personnage
    [SerializeField] private float speed;

    // Vitesse du saut du personnage
    [SerializeField] private float jumpPower;

    // Collision
    private Rigidbody2D body;
    // Animation
    private Animator anim;

    // Collision
    private BoxCollider2D boxCollider;
    // Jump
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Temps d'attente pour sauter à nouveau
    private float wallJumpCooldown;

    private float horizontalInput;


    private void Awake()
    {
        // Prend les références de Rigidbody2D et d'Animator pour l'objet PlayerMovement
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Obtenir la nouvelle position du corps dès qu'il change de place
    private void Update()
    {
        // Evite d'écrire à chaque fois Input.GetAxis()
        horizontalInput = Input.GetAxis("Horizontal");

        
        


        // Si le personnage bouge à droite -> changement d'animation
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new UnityEngine.Vector2(1, 1);
        }
        // Si le personnage bouge à gauche -> changement d'animation
        else if (horizontalInput < 0)
        {
            transform.localScale = new UnityEngine.Vector2(-1, 1);
        }

        // Animation quand le personnage court
        anim.SetBool("run", horizontalInput != 0);

        // Animation quand le personnage saute
        anim.SetBool("grounded", isGrounded());

        // Bouton espace -> Saut
        if (wallJumpCooldown > 0.2f)
        {
            body.linearVelocity = new UnityEngine.Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGrounded())
            {
                // Le joueur sera collé au mur
                body.gravityScale = 0;
                body.linearVelocity = UnityEngine.Vector2.zero;
            }
            else
            {
                body.gravityScale = 3;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    // Fonction Jump
    private void Jump()
    {

        if (isGrounded())
        {
            // Saut                                       //x                  //y
            body.linearVelocity = new UnityEngine.Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new UnityEngine.Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            else
            {
                body.linearVelocity = new UnityEngine.Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private bool isGrounded()
    {
        // BoxCasting permet d'identifier si le joueur est sur terre même si il se trouve sur une plateforme -> Avant c'était possible que sur le sol principal

        // Origine de la box, sa taille, l'angle, direction, distance, 
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    

     private bool onWall()
    {
        // Origine de la box, sa taille, l'angle, direction, distance, 
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new UnityEngine.Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
