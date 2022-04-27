using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        GoldController goldController = other.GetComponent<GoldController>();
        if(goldController != null)
        {

            goldController.teleport(new Vector2(destination.transform.position.x, destination.transform.position.y));
        }
    }
}
