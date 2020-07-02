using UnityEngine;
using System.Collections;

public class CharacterController2D: MonoBehaviour
{

    public float speed;              

    //private Rigidbody2D rb2d;    
    private Animator anim;
    private bool facingRight;
    private SpriteRenderer renderer2D;

    // Use this for initialization
    void Awake()
    {        
        //rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer2D = transform.GetComponent<SpriteRenderer>();
        facingRight = true;
    }
    
    void Update()
    { //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (moveHorizontal == 1f)
        {
            if (!facingRight)
                renderer2D.flipX = false;

            transform.Translate(Vector2.right * (Time.deltaTime * speed));
            anim.SetTrigger("Run");

        }else if (moveHorizontal == -1f){

            renderer2D.flipX = true;
            facingRight = false;
            transform.Translate(Vector2.left * (Time.deltaTime * speed));            
            anim.SetTrigger("Run");
        }
        else
        {
            anim.SetTrigger("Idle");
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