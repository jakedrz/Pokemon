using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    Animator animator;
    float horizontal, vertical;
    Rigidbody2D rigidBody;
    enum Direction {Up, Right, Down, Left};
    Direction lastDirection, currentDirection;
    string stringDirection;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        // horizontal = Input.GetAxis("Horizontal");
        // vertical = Input.GetAxis("Vertical");
        // Vector2 input = new Vector2(horizontal, vertical);
        Vector2 input;
        currentDirection = GetDirectionInput(out input);
        if(Mathf.Approximately(input.magnitude, 0))
        {
            
            switch(lastDirection)
            {
                case Direction.Up:
                    Debug.Log(Mathf.Ceil(position.y) + " " + position.y + " " + Mathf.Approximately(Mathf.Ceil(position.y), position.y));
                    if(Mathf.Ceil(position.y) > position.y)
                        input.y = 1;
                    break;
                case Direction.Right:
                    break;
                case Direction.Down:
                    break;
                default:
                    break;
            }
        }
        else
        {
            lastDirection = currentDirection;
        }
        Vector2 move = new Vector2(position.x + input.x * speed * Time.deltaTime, position.y + input.y * speed * Time.deltaTime);
        rigidBody.MovePosition(move);

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
            {direction = Direction.Left;
            input.y = 0;}
        else if((135f <= angle) && (angle > -135f))
            {direction = Direction.Down;
            input.x = 0;}
        else if((-135f <= angle) && (angle < -45f))
            {direction = Direction.Right;
            input.y = 0;}
        else
            {direction = Direction.Up;
            input.x = 0;}
        
        return direction;

        


    }
}
