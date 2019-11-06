using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainObject : MonoBehaviour
{

    private Rigidbody2D rb;
    private HingeJoint2D hj;
    private Transform tr;
    private SpriteRenderer sr;
    private int numberID;
    private int knots;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        hj = GetComponent<HingeJoint2D>();
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected void SetKnot(int iterationsLeft)
    {

        knots = iterationsLeft;
    }

    protected void SetID(int number)
    {

        numberID = number;
    }

    protected int GetID()
    {

        return numberID;
    }

    protected void SetTransform(Vector2 newPosition)
    {

        tr.transform.Translate(newPosition);
    }

    protected void ConnectBody(Rigidbody2D anchor)
    {

        hj.connectedBody = anchor;
    }

    protected void kill()
    {

        Destroy(this);
    }

    protected void AndAagin()
    {

        //Create child here
    }
}
