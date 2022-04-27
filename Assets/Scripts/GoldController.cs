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
        if (!Mathf.Approximately(input.magnitude, 0) && !isMoving)
        {
            lastPosition = rigidbody.position;
            lastDirection = currentDirection;
            isMoving = true;
            Vector2 velocity = new Vector2(input.x, input.y);
            animator.SetBool("Moving", isMoving);
            animator.SetFloat("Move X", input.x);
            animator.SetFloat("Move Y", input.y);
            StartCoroutine(Move(velocity));
        }
        else if(!isMoving)
        {
            animator.SetBool("Moving", isMoving);
            rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    IEnumerator Move(Vector2 velocity)
    {
        //keep moving until approximately 1 square away
        rigidbody.velocity = velocity * speed;
        
        while ((lastPosition - rigidbody.position).magnitude < 1)
        {
            if(collided)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    
        Vector2 p = transform.position;
        p.x = Mathf.Round(position.x);
        p.y = Mathf.Round(position.y);
        rigidbody.MovePosition(p);

        Vector2 input;
        GetDirectionInput(out input);
        //if input is none
        if (input.magnitude == 0)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            //stop animating and stop moving
            animator.SetBool("Moving", false);
        }
        isMoving = false;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        collided = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        collided = false;
    }
}