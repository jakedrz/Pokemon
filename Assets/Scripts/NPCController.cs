using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum Direction{Up, Right, Down, Left};
    public Direction direction;
    public bool wander;
    public int stride;
    public float speed = 3f;
    public float pauseMinimum = 5f;
    public float pauseMaximum = 5f;
    float pauseTime;
    Animator animator;
    Rigidbody2D rigidbody; 
    BoxCollider2D collider;
    Vector2 origin;
    Vector2 destination;
    bool moving;
    bool leaving;
    float lastTime;
    float t;

    
    // Start is called before the first frame update
    void Start()
    {
        pauseTime = Random.Range(pauseMinimum, pauseMaximum);
        lastTime = Time.fixedTime;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        origin = rigidbody.position;
        switch(direction)
        {
            case  Direction.Up:
                destination = new Vector2(origin.x, origin.y + stride);
                break;
            case Direction.Right:
                destination = new Vector2(origin.x + stride, origin.y);
                break;
            case Direction.Down:
                destination = new Vector2(origin.x, origin.y - stride);
                break;
            default:
                destination = new Vector2(origin.x - stride, origin.y);
                break;
        }
        animator.SetBool("Moving", false);
        look(direction);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(wander)
        {
            float timespan = Time.fixedTime - lastTime;
            if(moving)
            {
                float velX, velY;
                t += speed * Time.deltaTime / stride;
                if(leaving)
                {
                    rigidbody.MovePosition(Vector2.Lerp(origin, destination, t));
                    velX = (rigidbody.position - origin).normalized.x;
                    velY = (rigidbody.position - origin).normalized.y;
                }
                else
                {
                    rigidbody.MovePosition(Vector2.Lerp(destination, origin, t));
                    velX = -(rigidbody.position - origin).normalized.x;
                    velY = -(rigidbody.position - origin).normalized.y;
                }
                animator.SetBool("Moving", true);
                if(t >= 1)
                {
                    moving = false;
                    animator.SetBool("Moving", false);
                    lastTime = Time.fixedTime;
                    t = 0;
                    pauseTime = Random.Range(pauseMinimum, pauseMaximum);
                }
                else
                {
                    animator.SetFloat("Move X", velX);
                    animator.SetFloat("Move Y", velY);
                }
            }
            else
            {
                if(timespan >= pauseTime)
                {
                    //Debug.Log("timespan: " + timespan + " pauseTime: " + pauseTime);
                    if(stride != 0)
                    {
                        moving = true;
                        if(Mathf.Approximately(Mathf.Round(origin.x), Mathf.Round(rigidbody.position.x)) && Mathf.Approximately(Mathf.Round(origin.y), Mathf.Round(rigidbody.position.y)))
                        {
                            leaving = true;
                        }
                        else
                            leaving = false;
                    }
                    else
                    { 
                        Direction dir = (Direction)Random.Range(0, 3);
                        look(dir);
                        Debug.Log(dir);
                        pauseTime = Random.Range(pauseMinimum, pauseMaximum);
                        lastTime = Time.fixedTime;
                    }
                }
            }
        }
    }

    public void look(Direction direction)
    {
        //animator.SetBool("Moving", true);
        switch(direction)
        {
            case Direction.Up:
                animator.SetFloat("Move Y", 1f);
                break;
            case Direction.Right:
                animator.SetFloat("Move X", 1f);
                break;
            case Direction.Down:
                animator.SetFloat("Move Y", -1f);
                break;
            case Direction.Left:
                animator.SetFloat("Move X", -1f);
                break;
        }
        // animator.SetBool("Moving", true);
        // animator.SetBool("Moving", false);
    }

    public Vector2 position
    {
        get
        {
            return rigidbody.position;
        }
    }
}
