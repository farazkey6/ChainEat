using UnityEngine;
using System.Collections;

public enum Direction
{
    horizontal, vertical
}

public class MovingPlatform : MonoBehaviour
{

    public Direction direction;

    public float moveSpeed;

    public float positiveBoundary;

    public float negativeBoundary;

    public bool movePositive;

    // Update is called once per frame
    void Update()
    {
        float position = direction == Direction.horizontal ? transform.position.x : transform.position.y;

        // Turn on boundaries
        if (position >= positiveBoundary)
        {
            movePositive = false;
        }
        if (position <= negativeBoundary)
        {
            movePositive = true;
        }

        float newPosition = movePositive ? position + moveSpeed * Time.deltaTime : position - moveSpeed * Time.deltaTime;

        if (direction == Direction.horizontal)
            transform.position = new Vector2(newPosition, transform.position.y);
        else
            transform.position = new Vector2(transform.position.y, newPosition);
    }
}
