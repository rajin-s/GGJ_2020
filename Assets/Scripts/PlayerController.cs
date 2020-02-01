using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] float acceleration = 10.0f;
    [SerializeField] float damping = 10.0f;

    Rigidbody2D body;
    Vector2 input;
    bool hasInput;

    void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        input.Normalize();
        if (input.sqrMagnitude > 0.1f)
        {
            hasInput = true;
        }
        else
        {
            hasInput = false;
        }
    }

    void FixedUpdate() 
    {
        if (hasInput)
        {
            body.velocity = Vector2.ClampMagnitude(body.velocity + input * acceleration * Time.fixedDeltaTime, maxSpeed);
        }
        else
        {
            body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, Time.fixedDeltaTime * damping);
        }
    }
}
