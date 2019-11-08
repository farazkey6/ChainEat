using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public float speed;
    public float jumpForce;


    public GameObject head;
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftLeg;
    public GameObject rightLeg;


    public bool grounded;
    public bool canJump;

    // Update is called once per frame
    void Update()
    {
        Moving();
        if (leftLeg.gameObject.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")) || rightLeg.gameObject.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            grounded = true;
        } else
        {
            grounded = false;
            canJump = false;
        }
        if (grounded == true)
        {
            leftLeg.gameObject.GetComponent<Rigidbody2D>().mass = 1;
            rightLeg.gameObject.GetComponent<Rigidbody2D>().mass = 1;
            canJump = true;
        }
        if (grounded == false)
        {
            leftLeg.gameObject.GetComponent<Rigidbody2D>().mass = 5;
            rightLeg.gameObject.GetComponent<Rigidbody2D>().mass = 5;
        }
    }

    private void Moving()
    {
        if (Input.GetKey("d"))
        {
            transform.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Time.deltaTime * speed);
        }
        if (Input.GetKey("a"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown("space"))
        {
            if (canJump == true && grounded == true)
            {
                Jump(jumpForce);
                canJump = false;
                grounded = false;
            }
            
        }
    }

    private void Jump(float f)
    {
        
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, f), ForceMode2D.Impulse);

        canJump = false;
        grounded = false;
    }

    private void Grab()
    {

    }
    

    private void FixedUpdate()
    {
        
    }
}
