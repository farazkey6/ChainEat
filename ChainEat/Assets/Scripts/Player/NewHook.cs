using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewHook : MonoBehaviour
{
    public GameObject anchorPoint;
    private Rigidbody2D anchorRB;
    private SpriteRenderer anchorSprite;

    public DistanceJoint2D playerJoint;
    public PlayerController playerController;
    private Vector2 playerPosition;
    private bool isChained;

    public Transform target;
    public SpriteRenderer targetSprite;

    public LineRenderer chainRenderer;
    public LayerMask chainLayerMask; //Which layers the chain reacts to(Must add each in unity)
    public float chainLength;
    private List<Vector2> chainPositions = new List<Vector2>();
    private bool canHook;
    public float chainPieceSize;
    
    void Awake()
    {

        playerJoint.enabled = false;
        playerPosition = transform.position;
        anchorRB = anchorPoint.GetComponent<Rigidbody2D>();
        anchorSprite = anchorPoint.GetComponent<SpriteRenderer>();
        chainRenderer = playerController.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 aimDirection = TakeAim();
        HandleInput(aimDirection);
        //UpdateHook();
    }

    private void SetTargetPosition(float aimAngle)
    {
        if (!targetSprite.enabled)
        {
            targetSprite.enabled = true;
        }

        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var targetPosition = new Vector3(x, y, 0);
        target.transform.position = targetPosition;
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {

            if (isChained) return;
            chainRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, chainLength, chainLayerMask);


            if (hit.collider != null)
            {
                isChained = true;
                if (!chainPositions.Contains(hit.point))
                {

                    //Jump slightly after successful hook(or not lol) then hook
                    /*
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1f), ForceMode2D.Impulse);
                    chainPositions.Add(hit.point);
                    playerJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    playerJoint.enabled = true;
                    anchorSprite.enabled = true;
                    */
                    playerJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    chainRenderer.positionCount = (int)(playerJoint.distance / chainPieceSize);
                    for (int pieceCount = 0; pieceCount < chainRenderer.positionCount; pieceCount++)
                    {
                        print(pieceCount);
                        if (pieceCount == 0)
                        {
                            
                        }else if(pieceCount == chainRenderer.positionCount - 1)
                        {


                        }
                        else
                        {

                            chainRenderer.SetPosition (pieceCount, new Vector2 (transform.position.x + pieceCount * chainPieceSize * aimDirection.x, transform.position.y + pieceCount * chainPieceSize * aimDirection.y));
                        }
                    }
                    anchorSprite.enabled = true;
                    playerJoint.enabled = true;
                }
            }

            else
            {
                chainRenderer.enabled = false;
                isChained = false;
                playerJoint.enabled = false;
            }
        }
        else if (!Input.GetMouseButton(0))
        {

            ResetHook();
        }
    }

    private Vector2 TakeAim()
    {

        //Aim with mouse
        var worldMousePosition =
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;

        if (!isChained)
        {
            SetTargetPosition(aimAngle);
        }
        else
        {
            targetSprite.enabled = false;
        }

        return aimDirection;
    }

    private void UpdateHook()
    {
        //return if nothing connects
        if (!isChained)
        {
            return;
        }

        //find the correct position
        chainRenderer.positionCount = chainPositions.Count + 1;

        for (var i = chainRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != chainRenderer.positionCount - 1)
            {
                chainRenderer.SetPosition(i, chainPositions[i]);

                //set anchor
                if (i == chainPositions.Count - 1 || chainPositions.Count == 1)
                {
                    var tempChainPosition = chainPositions[chainPositions.Count - 1];
                    if (chainPositions.Count == 1)
                    {
                        anchorRB.transform.position = tempChainPosition;
                        if (!canHook)
                        {
                            playerJoint.distance = Vector2.Distance(transform.position, tempChainPosition);
                            canHook = true;
                        }
                    }
                    else
                    {
                        anchorRB.transform.position = tempChainPosition;
                        if (!canHook)
                        {
                            playerJoint.distance = Vector2.Distance(transform.position, tempChainPosition);
                            canHook = true;
                        }
                    }
                }
                // fixes some strange behaviours
                else if (i - 1 == chainPositions.IndexOf(chainPositions.Last()))
                {
                    var tempChainPosition = chainPositions.Last();
                    anchorRB.transform.position = tempChainPosition;
                    if (!canHook)
                    {
                        playerJoint.distance = Vector2.Distance(transform.position, tempChainPosition);
                        canHook = true;
                    }
                }
            }
            else
            {
                // yay
                chainRenderer.SetPosition(i, transform.position);
            }
        }
    }

    private void ResetHook()
    {
        playerJoint.enabled = false;
        isChained = false;
        playerController.isSwinging = false;
        chainRenderer.positionCount = 2;
        chainRenderer.SetPosition(0, transform.position);
        chainRenderer.SetPosition(1, transform.position);
        chainPositions.Clear();
        anchorSprite.enabled = false;
    }
}
