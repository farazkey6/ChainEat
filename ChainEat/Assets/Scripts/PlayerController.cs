using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Move variables
    float horizontal_Move = 0f;
    public float moveSpeed;
    public float maxSpeed;

    //Collision detection for feet variables
    public CircleCollider2D c_Collider;
    public Rigidbody2D p_Rigidbody;

    //Jump variables
    bool jump = false;
    float fallModifier = 2f;
    float jumpModifier = 1.5f;
    bool grounded = false;
    public float jumpForce;

    //Swinging
    public bool isSwinging;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        horizontal_Move = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (p_Rigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {

            p_Rigidbody.velocity *= 0.9f;
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {

            jump = true;
        }

    }

    private void FixedUpdate()
    {
        
        //Must set Ground layer for jumpable surfaces or this won't work (rip english)
        if (c_Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {

            //Land();
            grounded = true;
        }
        else
        {

            grounded = false;
        }

        if (jump && grounded)
        {

            //Unity is dumb so here's a dumb solution to a dumb bug.. geez
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

            Jump(jumpForce);
        }

        if (p_Rigidbody.velocity.y < 0)
        {

            p_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallModifier - 1) * Time.deltaTime;
        }
        else if (p_Rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {

            p_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (jumpModifier - 1) * Time.deltaTime;
        }

        Vector2 movement = new Vector2(horizontal_Move, 0f);
        Move(movement);
    }

    private void Move(Vector3 movement)
    {

        //This is so we can launch our character forward ;)
        if (horizontal_Move * transform.localScale.x < 0f)
        {

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        if (Mathf.Abs(horizontal_Move) > 0)
        {
            
            transform.position += movement * Time.fixedDeltaTime;
        }
    }

    private void Jump(float force)
    {
        
        p_Rigidbody.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);

        jump = false;
        grounded = false;
    }

    //Useless for now
    private void Land()
    {

        grounded = true;
    }
}
