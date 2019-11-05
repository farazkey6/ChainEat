using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContainter : MonoBehaviour
{

    public int health = 500; // Player Health
    public int enemyHealth = 100; // Enemy Health

    public Transform parent; // what the weapon will attach to
    private Vector3 parentPos; // the position of the parent

    // Update is called once per frame
    void Update()
    {
        parentPos = parent.position;
        Aim();
    }

    protected void Aim()
    {
        transform.SetParent(parent);
        transform.position = (parentPos);
    }

    protected void WeaponStats(float damage, float speed, float throwForce)
    {
        Vector3 mouseToScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (Vector2)(mouseToScreen - transform.position);
        float dirSpeed = speed;
        speed = transform.GetComponent<Rigidbody2D>().AddForce(dir * dirSpeed);

    }
}