using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    Animator animator;
    float horizontal, vertical;
    Rigidbody2D rigidbody;
    enum Direction {Up, Right, Down, Left};
    Direction lastDirection, currentDirection;
    Vector2 lastPosition;
    string stringDirection;
    bool isMoving;

    public float speed = 3f;
    public float PPU;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called regularly, regardless of frame
    void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 input;
        currentDirection = GetDirectionInput(out input);
        if(Mathf.Approximately(input.magnitude, 0))
        {
            switch(lastDirection)
            {
                case Direction.Up:
                    if(Mathf.Ceil(lastPosition.y) > (position.y + 1f / (PPU * 2)))
                        input.y = 1;
                    else
                        input.y = 0;
                        rigidbody.MovePosition(new Vector2(position.x, position.y + 1f / (PPU)));
                    break;
                case Direction.Right:
                    if(Mathf.Ceil(lastPosition.x) > (position.x + 1f / (PPU * 2)))
                        input.x = 1;
                    else
                        input.x = 0;
                        rigidbody.MovePosition(new Vector2(position.x + 1f / (PPU), position.y));
                    break;
                case Direction.Down:
                    if(Mathf.Floor(lastPosition.y) < (position.y - 1f / (PPU * 2)))
                        input.y = -1;
                    else
                        input.y = 0;
                        rigidbody.MovePosition(new Vector2(position.x, position.y - 1f / (PPU)));
                    break;
                default:
                    if(Mathf.Floor(lastPosition.x) < position.x - 1f / (PPU * 2))
                        input.x = -1;
                    else
                        input.x = 0;
                        rigidbody.MovePosition(new Vector2(position.x - 1f / (PPU), position.y));
                    break;
            }
        }
        else
        {
            lastDirection = currentDirection;
            lastPosition = position;
        }
        Vector2 move = new Vector2(position.x + input.x * speed * Time.deltaTime, position.y + input.y * speed * Time.deltaTime);
        rigidbody.MovePosition(move);

        move.Normalize();

        bool isMoving = (Mathf.Approximately(input.magnitude, 0)) ? false : true;
        if(isMoving)
        {
            animator.SetBool("Moving", isMoving);
            animator.SetFloat("Move X", input.x);
            animator.SetFloat("Move Y", input.y);
        }
        else
        {
            animator.SetBool("Moving", isMoving);
        }
    }

    Direction GetDirectionInput(out Vector2 input)
    {
        Direction direction;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        input = new Vector2(horizontal, vertical);
        float angle = Vector2.SignedAngle(Vector2.up, input);
        if((45f <= angle) && (angle < 135f))
        {
            direction = Direction.Left;
            input.y = 0;
        }
        else if((135f <= angle) && (angle > -135f))
        {
            direction = Direction.Down;
            input.x = 0;
        }
        else if((-135f <= angle) && (angle < -45f))
        {
            direction = Direction.Right;
            input.y = 0;
        }
        else
        {
            direction = Direction.Up;
            input.x = 0;
        }
        return direction;
    }

    public Vector2 position
    {
        get
        {
            return rigidbody.position;
        }
    }

    public bool transient
    {
        // TODO: write transient function
        get
        {
            return false;
        }
    }

    public void teleport(Vector2 dest)
    {
        transform.position = dest;
    }
}
