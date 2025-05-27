using NUnit.Framework.Internal;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private float speed;
   private Rigidbody2D body;
   private void Awake() 
   {
      body = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      if (Input.GetKey(KeyCode.D))
      {
         body.linearVelocity = new Vector2(speed, body.linearVelocity.y);
      }
      else if (Input.GetKey(KeyCode.Q))
      {
         body.linearVelocity = new Vector2(-speed, body.linearVelocity.y);
      }
      if (Input.GetKeyDown(KeyCode.Space))
      {
         body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
      }
   }
}
