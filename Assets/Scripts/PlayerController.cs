using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float damping = 10.0f;

    [SerializeField] private bool useAcceleration = false;

    [SerializeField] string inputNamePrefix = "";
    private string horizontalInputName;
    private string verticalInputName;
    private string attractInputName;

    private Rigidbody2D body;
    private SheepAttractor sheepAttractor;

    [SerializeField] private Vector2 input;
    private bool hasInput;

    void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        sheepAttractor = GetComponentInChildren<SheepAttractor>();

        horizontalInputName = inputNamePrefix + "Horizontal";
        verticalInputName = inputNamePrefix + "Vertical";
        attractInputName = inputNamePrefix + "Attract";
    }

    void Update()
    {
        input.x = Input.GetAxis(horizontalInputName);
        input.y = Input.GetAxis(verticalInputName);

        input = Vector2.ClampMagnitude(input, 1);
        if (input.sqrMagnitude > 0.005f)
        {
            hasInput = true;
        }
        else
        {
            hasInput = false;
        }

        if (Input.GetButton(attractInputName))
        {
            sheepAttractor.SetActive(true);
        }
        else
        {
            sheepAttractor.SetActive(false);
        }
    }

    void FixedUpdate() 
    {
        if (useAcceleration)
        {
            if (hasInput)
            {
                float max = maxSpeed * input.magnitude;
                body.velocity = Vector2.ClampMagnitude(body.velocity + input * acceleration * Time.fixedDeltaTime, max);
            }
            else
            {
                body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, Time.fixedDeltaTime * damping);
            }
        }
        else
        {
            body.velocity = input * maxSpeed;
        }
    }
}