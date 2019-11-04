using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private float throwForce = 600;
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

            if (Input.GetMouseButtonDown(1))
            {
                //throw
                item.GetComponent<Rigidbody2D>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
            
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

    // If object is clicked
    void PickupObject()
    {
        if (Input.GetKey("e"))
        {
            // Check if the player is in range of object
            if (distance <= 1f)
            {
                // When picked up
                if (isHolding == false)
                {
                    isHolding = true;
                    item.GetComponent<Rigidbody2D>().gravityScale = 0;
                    item.GetComponent<Rigidbody2D>().collisionDetectionMode = 0;
                    
                }

            }
        }
    }

    void Passthrough()
    {
        if (isHolding == true)
        {
            item.GetComponent<Collider2D>().isTrigger = true;
        } else
        {
            if (isHolding == false)
            {
                item.GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }

    void ThrowObject()
    {
        // Chck if the player is holding an object
        if (isHolding == true)
        {
            // Throw with Left Click
            if (Input.GetMouseButton(0))
            {
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (Vector2)((worldMousePos - transform.position));
                dir.Normalize();
                item.GetComponent<Rigidbody2D>().AddForce(dir * throwForce);
                isHolding = false;
            }
            else
            {
                // If isHolding = False, return nothing
                return;
            }
        }
        
    }

    private void Aim(Vector3 target)
    {
        Vector2 dir = transform.position - target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    void DropObject()
    {
        if (isHolding == true)
        {
            if (Input.GetMouseButton(1))
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
