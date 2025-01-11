using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;

    private bool swiping = false;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            if (Input.GetMouseButtonDown(0))//Allow object to interact with game based on mouse down 
            {
                UpdateComponents(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UpdateComponents(false);
            }

            //If interactive, make sure to update position based on mouse position
            if (swiping)
            {
                UpdateMousePosition();
            }
        }
    }

    //Updates objects position to where the mouse is currently at
    void UpdateMousePosition()
    {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    //Turns effects on and off and interaction based on treu or false
    void UpdateComponents(bool status)
    {
        swiping = status;
        trail.enabled = status;
        col.enabled = status;
    }

    //Destroy object (the balls and skulls) that collide with this object
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Target>())
        {
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }
}
