using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement = new Vector2();

    Rigidbody2D rb2d;

    [SerializeField]
    int speed;

    void Awake() 
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = (Input.GetAxis("Horizontal"));
        movement.y = (Input.GetAxis("Vertical"));
    }

    void FixedUpdate() 
    {
        rb2d.AddForce(movement * speed);
    }
}
