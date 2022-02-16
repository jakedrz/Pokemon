using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool wander;
    //public bool 
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Moving", true);
        animator.SetFloat("Move X", -1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
