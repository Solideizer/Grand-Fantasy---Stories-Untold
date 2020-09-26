using UnityEngine;

namespace NOT_IMPLEMENTED
{
    public class CharacterController2D: MonoBehaviour
    {
        public float speed;            
      
        private Animator _anim;
        private bool _facingRight;
        private SpriteRenderer _renderer2D;

        // Use this for initialization
        void Awake()
        {     
        
            _anim = GetComponent<Animator>();
            _renderer2D = transform.GetComponent<SpriteRenderer>();
            _facingRight = true;
        }
    
        void Update()
        { //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (moveHorizontal == 1f)
            {
                if (!_facingRight)
                    _renderer2D.flipX = false;

                transform.Translate(Vector2.right * (Time.deltaTime * speed));
                _anim.SetTrigger("Run");

            }else if (moveHorizontal == -1f){

                _renderer2D.flipX = true;
                _facingRight = false;
                transform.Translate(Vector2.left * (Time.deltaTime * speed));            
                _anim.SetTrigger("Run");
            }
            else
            {
                _anim.SetTrigger("Idle");
            }

        
        

            //Store the current vertical input in the float moveVertical.
            //float moveVertical = Input.GetAxisRaw("Vertical");

            //Use the two store floats to create a new Vector2 variable movement.
            //Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            //rb2d.AddForce(movement * speed, ForceMode2D.Impulse);
            //if (moveVertical > 0 || moveHorizontal > 0)
            //{
            //    anim.SetTrigger("Run");
            //}
            //else
            //{
            //    anim.SetTrigger("Idle");
            //}


        }

   

    }
}