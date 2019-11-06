using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float throwForce;
    private Vector2 objectPos;
    private float distance;

    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;

    

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(item.transform.position, tempParent.transform.position);
        if(distance >= 1f)
        {
            isHolding = false;
        }
        // Check if isHolding and if true to set the parent of the weapon (Rock)
        if (isHolding == true)
        {
            
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            item.transform.SetParent(tempParent.transform);
            item.transform.position = tempParent.transform.position;
            
        }
        else
        {
            // If object isn't being held
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody2D>().gravityScale = 1;
            item.transform.position = objectPos;
        }
        Passthrough();
        PickupObject();
        ThrowObject();
        

        
        
    }

    
    void PickupObject()
    {
        // Check if the player isn't holding anything already
        if (tempParent.transform.childCount == 0)
        {
            if (Input.GetKey("e"))
            {

                // If the distance between the item and player is less than 1m
                if (distance <= 1f)
                {
                    isHolding = true;

                    // turn off the items gravity and it wont detect any collisions
                    item.GetComponent<Rigidbody2D>().gravityScale = 0;
                    item.GetComponent<Rigidbody2D>().collisionDetectionMode = 0;

                }

            }
        // if the character is holding something, do nothing
        } else if (tempParent.transform.childCount > 0)
        {
            return;
        }
    }

    // Allow the object to passthrough the player when the item is being held
    void Passthrough()
    {
        if (isHolding == true)
        {
            item.GetComponent<Collider2D>().isTrigger = true;
        } else if (isHolding == false)
        {
            item.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    void ThrowObject()
    {
        // Chck if the player is holding an object
        if (isHolding == true)
        {
            // Throw with Left Click
            if (Input.GetMouseButton(1))
            {
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (Vector2)((worldMousePos - transform.position));
                dir.Normalize();
                item.GetComponent<Rigidbody2D>().AddForce(dir * throwForce);
                item.GetComponent<Rigidbody2D>().AddTorque(-100);
                isHolding = false;
            }
        } else if (isHolding == false)
        {
            return;
        }
        //if (tempParent.transform.childCount >= 1)
        //{
        //    if (Input.GetKey("e"))
        //    {
        //        // Drop held object
        //        isHolding = false;
        //    }
        //}
    }

    void DropObject()
    {
        if (isHolding == true)
        {
            if (Input.GetKey("e"))
            {
                isHolding = false;
            }
        }
    }

    // If rock collides with enemy
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
