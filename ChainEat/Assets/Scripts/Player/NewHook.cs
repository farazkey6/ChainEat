using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewHook : MonoBehaviour
{
    public GameObject anchorPoint;
    public GameObject chainPrefab;
    private GameObject chainHandle;
    private Rigidbody2D anchorRB;
    private SpriteRenderer anchorSprite;

    public DistanceJoint2D playerJoint;
    public PlayerController playerController;
    private Vector2 playerPosition;
    private bool isChained;

    public Transform target;
    public SpriteRenderer targetSprite;

    //public LineRenderer chainRenderer;
    public LayerMask chainLayerMask; //Which layers the chain reacts to(Must add each in unity)
    public float chainLength;
    private List<Vector2> chainPositions = new List<Vector2>();
    private float chainPieceSize;
    public HingeJoint2D jointAnchor;
    
    void Awake()
    {

        playerJoint.enabled = false;
        playerPosition = transform.position;
        anchorRB = anchorPoint.GetComponent<Rigidbody2D>();
        anchorSprite = anchorPoint.GetComponent<SpriteRenderer>();
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
        UpdateChain();
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

            var hit = Physics2D.Raycast(playerPosition, aimDirection, chainLength, chainLayerMask);
            chainHandle = Instantiate(chainPrefab);

            if (hit.collider != null)
            {
                isChained = true;
                if (!chainPositions.Contains(hit.point))
                {
                    
                    playerJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    chainHandle.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    var trueSize = chainHandle.GetComponent<AutoChain>().GetTrueSize();
                    chainPieceSize = trueSize;
                    chainPieceSize = 0.5f;
                    int chainKnots = (int)(playerJoint.distance / chainPieceSize);
                    Vector3 offsetPos = new Vector3(chainPieceSize * aimDirection.x, chainPieceSize * aimDirection.y, 0);
                    chainHandle.GetComponent<AutoChain>().SetOffset(offsetPos);
                    chainHandle.GetComponent<AutoChain>().SetKnot(chainKnots);
                    chainHandle.GetComponent<AutoChain>().PassHook(hit.rigidbody);
                    //chainHandle.GetComponent<HingeJoint2D>().connectedBody = anchorRB;
                   // chainHandle.GetComponent<HingeJoint2D>().enabled = true;
                    chainHandle.GetComponent<DistanceJoint2D>().connectedBody = anchorRB;
                    chainHandle.GetComponent<DistanceJoint2D>().enabled = true;
                    chainHandle.GetComponent<Transform>().transform.position = GetComponent<Transform>().position + offsetPos;
                    chainHandle.GetComponent<AutoChain>().BootUp();
                    
                    anchorSprite.enabled = true;
                    playerJoint.enabled = true;
                }
            }

            else
            {
                
                isChained = false;
                playerJoint.enabled = false;

                GameObject[] gos = GameObject.FindGameObjectsWithTag("Chain");
                foreach(GameObject go in gos)
                    Destroy(go);
            }
        }
        else if (!Input.GetMouseButton(0) && isChained)
        {
            DestroyChain(chainHandle);
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

    private void UpdateChain()
    {

        if (!isChained)
            return;


        anchorRB.transform.position = chainHandle.transform.position;
        playerJoint.distance = Vector2.Distance(transform.position, chainHandle.transform.position);

        var child = transform.GetChild(0);
        chainHandle.GetComponent<Rigidbody2D>().position = child.transform.position;
        chainHandle.GetComponent<DistanceJoint2D>().distance = Vector2.Distance(transform.position, child.transform.position);
        chainHandle.GetComponent<AutoChain>().UpdatePosition();
    }

    private void DestroyChain(GameObject chain)
    {

        isChained = false;
        playerJoint.enabled = false;
        playerController.isSwinging = false;
        anchorSprite.enabled = false;
        chainPositions.Clear();
        chain.GetComponent<AutoChain>().Kill();
        Destroy(chain);
    }
}
