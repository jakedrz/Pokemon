using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    Animator animator;
    float horizontal,
        vertical;
    Rigidbody2D rigidbody;

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    };

    Direction lastDirection,
        currentDirection;
    Vector2 lastPosition;
    string stringDirection;
    bool isMoving = false;
    bool collided = false;
    Vector2 directionConstrainer;

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
        Vector2 input;
        currentDirection = GetDirectionInput(out input);
        //if input
        if (!Mathf.Approximately(input.magnitude, 0) && isMoving == false)
        {
            lastPosition = rigidbody.position;
            lastDirection = currentDirection;
            isMoving = true;
            Vector2 velocity = new Vector2(
                input.x,
                input.y
            );
            animator.SetBool("Moving", true);
            animator.SetFloat("Move X", input.x);
            animator.SetFloat("Move Y", input.y);
            StartCoroutine(Move(velocity));
        }
        
    }

    IEnumerator Move(Vector2 velocity)
    {
        //rigidbody.MovePosition(move);
        while (((lastPosition - rigidbody.position).magnitude < 1 - (1f / (PPU * 2))) && collided == false)
        {
            rigidbody.MovePosition(
                //Vector2.MoveTowards(rigidbody.position, toPosition, speed * Time.fixedDeltaTime)
                rigidbody.position + velocity * speed * Time.fixedDeltaTime
            );
            yield return null;
        }
        //transform.position = toPosition;
        isMoving = false;
        Debug.Log("end coroutine");

        Vector2 input;
        GetDirectionInput(out input);
        if(input.magnitude == 0)
            animator.SetBool("Moving", false);

        if(true)
        {
            Vector2 p = transform.position;
            switch (lastDirection)
            {
                case Direction.Up:
                    p.y = Mathf.Round(position.y);
                    break;
                case Direction.Right:
                    p.x = Mathf.Round(position.x);
                    break;
                case Direction.Down:
                    p.y = Mathf.Round(position.y);
                    break;
                default:
                    p.x = Mathf.Round(position.x);
                    break;
            }
        rigidbody.MovePosition(p);
        }
    }

    Direction GetDirectionInput(out Vector2 input)
    {
        Direction direction;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        input = new Vector2(horizontal, vertical);
        float angle = Vector2.SignedAngle(Vector2.up, input);
        if ((45f <= angle) && (angle < 135f))
        {
            direction = Direction.Left;
            input.y = 0;
        }
        else if ((135f <= angle) && (angle > -135f))
        {
            direction = Direction.Down;
            input.x = 0;
        }
        else if ((-135f <= angle) && (angle < -45f))
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
        get { return rigidbody.position; }
    }

    public void teleport(Vector2 dest)
    {
        transform.position = dest;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collision Enter");
        if(!isMoving)
        {
            collided = true;
            isMoving = false;
            //animator.SetBool("Moving", false);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("Collision Exit");
        collided = false;
    }
}
