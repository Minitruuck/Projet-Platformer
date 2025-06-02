using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
// Pas besoin ?
using UnityEngine.Android;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] permet de modifier la valeur de speed depuis Unity
    [SerializeField] private float speed;
    // Collision
    private Rigidbody2D body;
    // Animation
    private Animator anim;

    // Jump
    private bool grounded;

    private void Awake()
    {
        // Prend les références de Rigidbody2D et d'Animator pour l'objet PlayerMovement
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Obtenir la nouvelle position du corps dès qu'il change de place
    private void Update()
    {
        // Evite d'écrire à chaque fois Input.GetAxis()
        float horizontalInput = Input.GetAxis("Horizontal");

        // Boutons Q et D du clavier                    // Abscisse (x) Ordonnée (y)
        // Valeur définie par Unity compris entre -1(Q) et 1(D) <- touches par défaut sur Unity
        body.linearVelocity = new UnityEngine.Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Bouton espace -> Saut
        // Si le joueur appuie sur la barre espace et que le personnage n'est pas en l'air, il peut sauter
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }


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
        anim.SetBool("grounded", grounded);
    }

    // Fonction Jump
    private void Jump()
    {
        // Saut                                       //x                  //y
        body.linearVelocity = new UnityEngine.Vector2(body.linearVelocity.x, speed);

        anim.SetTrigger("jump");

        // Le personnage n'est plus sur le sol
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
