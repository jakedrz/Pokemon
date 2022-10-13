using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    Animator animator;
    float horizontal,
        vertical;
    Rigidbody2D rigidbody;

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    };

    Vector2 lastDirection;
    Vector2 lastPosition;
    string stringDirection;
    bool isMoving = false;
    bool collided = false;
    bool fromRest = true;
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
        GetDirectionalInput(out input);
        //if input
        if (!Mathf.Approximately(input.magnitude, 0) && !isMoving)
        {
            isMoving = true;
            Vector2 velocity = new Vector2(input.x, input.y);
            StartCoroutine(Move(velocity));
        }
        else if(!isMoving)
        {
            animator.SetBool("Moving", isMoving);
            rigidbody.velocity = new Vector3(0, 0, 0);
            fromRest = true;
        }
    }

    IEnumerator Move(Vector2 velocity)
    {
        Vector2 input = new Vector2(0, 0);
        look(GetDirectionalVelocity(velocity));

        /* if currentDirection has changed and gold is moving from rest,
        make gold look in new direction, wait,
        then check to see if input is still pressed*/
        if((velocity != lastDirection) && fromRest)
        {
            yield return new WaitForSeconds(0.125f);
            lastDirection = velocity;
        }
        
        /* if there's input and the currentDirection is the same as the lastDirection,
           move gold */
        GetDirectionalInput(out input);
        if((!Mathf.Approximately(input.magnitude, 0)) && (velocity == input))
        {
            fromRest = false;
            lastPosition.x = Mathf.Round(rigidbody.position.x);
            lastPosition.y = Mathf.Round(rigidbody.position.y);
            
            //Vector2 velocity = new Vector2(input.x, input.y);
            animator.SetBool("Moving", isMoving);
            animator.SetFloat("Move X", input.x);
            animator.SetFloat("Move Y", input.y);
            
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
        }
        isMoving = false;
    }

    Direction GetDirectionalInput(out Vector2 input)
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
        input.x = Mathf.RoundToInt(input.x);
        input.y = Mathf.RoundToInt(input.y);
        return direction;
    }
    Direction GetDirectionalVelocity(Vector2 velocity)
    {
        Direction direction;
        float angle = Vector2.SignedAngle(Vector2.up, velocity);
        if ((45f <= angle) && (angle < 135f))
        {
            direction = Direction.Left;
        }
        else if ((135f <= angle) && (angle > -135f))
        {
            direction = Direction.Down;
        }
        else if ((-135f <= angle) && (angle < -45f))
        {
            direction = Direction.Right;
        }
        else
        {
            direction = Direction.Up;
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

    public void look(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                animator.SetFloat("Move Y", 1f);
                animator.SetFloat("Move X", 0f);
                break;
            case Direction.Right:
                animator.SetFloat("Move X", 1f);
                animator.SetFloat("Move Y", 0f);
                break;
            case Direction.Down:
                animator.SetFloat("Move Y", -1f);
                animator.SetFloat("Move X", 0f);
                break;
            case Direction.Left:
                animator.SetFloat("Move X", -1f);
                animator.SetFloat("Move Y", 0f);
                break;
        }
    }
}