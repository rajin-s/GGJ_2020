using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float damping = 10.0f;

    [SerializeField] string inputNamePrefix = "";
    private string horizontalInputName;
    private string verticalInputName;
    private string attractInputName;

    private Rigidbody2D body;
    private SheepAttractor sheepAttractor;

    private Vector2 input;
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

        input.Normalize();
        if (input.sqrMagnitude > 0.1f)
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
