using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;
    public Image fader;
    private Image imageFader;
    void Start()
    {
        Color fadeColor = Color.clear;
        imageFader = fader.GetComponent<Image>();
        imageFader.color = fadeColor;
        Debug.Log(imageFader.color);
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
