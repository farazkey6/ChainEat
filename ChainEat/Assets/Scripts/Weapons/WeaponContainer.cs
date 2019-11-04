using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    // gets the position of both the player and the rock to determine if the rock is in "pickup range" of the target.
    private Vector2 rockPos;
    private Vector2 playerPos;
    public float pickupRange;


    public Transform rockAttachPos;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        AttachToPlayer();
    }

    // When rock comes into contact with player, attach to wherever (Empty Object Named "RockAttachPos")
    protected void AttachToPlayer()
    {
        playerPos = target.position;
        if (playerPos.x < pickupRange)
        {
            transform.SetParent(rockAttachPos);
        }
    }

}
