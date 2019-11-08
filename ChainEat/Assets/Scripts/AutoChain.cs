using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoChain : MonoBehaviour
{
    public GameObject chainPrefab;

    private ChainObject co;
    private Rigidbody2D rb;
    private HingeJoint2D hj;
    private DistanceJoint2D dj;
    private Transform tr;
    private SpriteRenderer sr;
    private int knots;
    private Vector3 offsetPosition;
    private Rigidbody2D hook;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        hj = GetComponent<HingeJoint2D>();
        dj = GetComponent<DistanceJoint2D>();
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        co = GetComponent<ChainObject>();
    }

    public void BootUp()
    {

        if (knots > 0)
        {

            knots--;
            var child = Instantiate(chainPrefab, tr);
            child.GetComponent<Rigidbody2D>().gravityScale = 0f;
            child.GetComponent<AutoChain>().SetOffset(offsetPosition);
            child.GetComponent<AutoChain>().SetKnot(knots);
            child.GetComponent<AutoChain>().PassHook(hook);
            //var anchorPoint = child.GetComponent<Transform>().position - tr.position;
            //child.GetComponent<HingeJoint2D>().connectedAnchor = anchorPoint ;
            //child.GetComponent<HingeJoint2D>().connectedBody = rb;
            //child.GetComponent<HingeJoint2D>().enabled = true;/*
            child.GetComponent<DistanceJoint2D>().connectedBody = rb;
            child.GetComponent<DistanceJoint2D>().distance = 0.5f;
            child.GetComponent<DistanceJoint2D>().enabled = true;
            child.GetComponent<SpriteRenderer>().enabled = true;
            child.GetComponent<Transform>().transform.position = tr.position + offsetPosition;
            child.GetComponent<AutoChain>().BootUp();
        }else if (knots == 0)
        {

            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = hook;
        }
    }

    public float GetTrueSize()
    {

        return sr.size.x;
    }

    public void PassHook(Rigidbody2D newHook)
    {

        hook = newHook;
    }

    public void SetKnot(int iterationsLeft)
    {

        knots = iterationsLeft;
    }

    public void SetOffset (Vector3 offset)
    {

        offsetPosition = offset;
    }

    public void UpdatePosition()
    {

        if (knots != 0)
        {
            var child = transform.GetChild(0);
            rb.position = child.transform.position;
            dj.distance = Vector2.Distance (transform.position, child.transform.position);
            child.GetComponent<AutoChain>().UpdatePosition();
        }
    }

    public void Kill()
    {
        foreach (Transform child in transform)
        {

            Destroy(child.gameObject);
        }
        //Destroy(this);
    }
}
