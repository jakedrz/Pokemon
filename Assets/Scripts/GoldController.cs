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
            isMoving = true;
            Vector2 toPosition = new Vector2(position.x + input.x, position.y + input.y);
            animator.SetBool("Moving", true);
            animator.SetFloat("Move X", input.x);
            animator.SetFloat("Move Y", input.y);
            Debug.Log("Current position: X: " + position.x + " Y: " + position.y);
            Debug.Log("To position     : X: " + position.x + input.x + " Y: " + position.y + input.y);
            StartCoroutine(Move(toPosition));
        }
    }

    IEnumerator Move(Vector2 toPosition)
    {
        //rigidbody.MovePosition(move);
        Vector3 toPosition3 = new Vector3(toPosition.x, toPosition.y, 0);
        while((toPosition3 - transform.position).magnitude > Mathf.Epsilon)
        {
            rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, toPosition, speed * Time.deltaTime));
            yield return null;
        }
        isMoving = false;
        Debug.Log("end coroutine");
        
        // Vector2 input;
        // GetDirectionInput(out input);
        // if(input.magnitude == 0)
        //     animator.SetBool("Moving", false);
        


        // switch (lastDirection)
        // {
        //     case Direction.Up:
        //         //if dest square is still far away
        //         if (Mathf.Ceil(lastPosition.y) > (position.y + 1f / (PPU * 2)))
        //             input.y = 1;
        //         //if it's pretty close
        //         else
        //         {
        //             input.y = 0;
        //             //transform.position.y = Mathf.Round(position.y);
        //         }
        //         break;
        //     case Direction.Right:
        //         if (Mathf.Ceil(lastPosition.x) > (position.x + 1f / (PPU * 2)))
        //             input.x = 1;
        //         else
        //         {
        //             input.x = 0;
        //             //transform.position.x = Mathf.Round(position.x);
        //         }
        //         break;
        //     case Direction.Down:
        //         if (Mathf.Floor(lastPosition.y) < (position.y - 1f / (PPU * 2)))
        //             input.y = -1;
        //         else
        //         {
        //             input.y = 0;
        //             //transform.position.y = Mathf.Round(position.y);
        //         }
        //         break;
        //     default:
        //         if (Mathf.Floor(lastPosition.x) < position.x - 1f / (PPU * 2))
        //             input.x = -1;
        //         else
        //         {
        //             input.x = 0;
        //             //transform.position.x = Mathf.Round(position.x);
        //         }
        //         break;
        // }
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

    public bool transient
    {
        // TODO: write transient function
        get { return false; }
    }

    public void teleport(Vector2 dest)
    {
        transform.position = dest;
    }

    // void OnCollisionEnter2D(Collider2D other)
    // {
    //     animator.SetBool("Moving", false);
    // }
}
