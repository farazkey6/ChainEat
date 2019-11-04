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
    }

    // If object is clicked
    void OnMouseDown()
    {
        // Check if the player is in range of object
        if(distance <= 1f)
        {
            isHolding = true;
            item.GetComponent<Rigidbody2D>().gravityScale = 0;
            item.GetComponent<Rigidbody2D>().collisionDetectionMode = 0;
        }
        

    }
    void OnMouseUp()
    {
        isHolding = false;
    }

}
