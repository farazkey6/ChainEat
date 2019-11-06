using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoChain : MonoBehaviour
{
    public GameObject chainPrefab;

    private ChainObject co;
    private Rigidbody2D rb;
    private HingeJoint2D hj;
    private Transform tr;
    private SpriteRenderer sr;
    private int knots;
    private Vector3 pos;
    private Rigidbody2D hook;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        hj = GetComponent<HingeJoint2D>();
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        co = GetComponent<ChainObject>();
    }
    
    protected void BootUp()
    {

        if (knots > 0)
        {

            knots--;
            var child = Instantiate(chainPrefab, tr);
            child.GetComponent<Rigidbody2D>().gravityScale = 0f;
            child.GetComponent<AutoChain>().SetPos(pos);
            child.GetComponent<AutoChain>().SetKnot(knots);
            child.GetComponent<AutoChain>().PassHook(hook);
            child.GetComponent<HingeJoint2D>().connectedBody = rb;
            child.GetComponent<Transform>().transform.position = tr.position + new Vector3 (pos.x, pos.y, 0);
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

    protected void PassHook(Rigidbody2D newHook)
    {

        hook = newHook;
    }

    protected void SetKnot(int iterationsLeft)
    {

        knots = iterationsLeft;
    }

    protected void SetPos (Vector3 newPos)
    {

        pos = newPos;
    }

    protected void Kill()
    {
        foreach (Transform child in transform)
        {

            Destroy(child.gameObject);
        }
        Destroy(this);
    }
}
