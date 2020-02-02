using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float damping = 10.0f;

    [SerializeField] private bool useAcceleration = false;

    [SerializeField] private float externalDamping = 4.0f;

    [SerializeField] string inputNamePrefix = "";
    private string horizontalInputName;
    private string verticalInputName;
    private string attractInputName;

    private Rigidbody2D body;
    private SheepAttractor sheepAttractor;

    private Vector2 input;
    private Vector2 finalVelocity;

    private bool hasInput;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sheepAttractor = GetComponentInChildren<SheepAttractor>();

        horizontalInputName = inputNamePrefix + "Horizontal";
        verticalInputName = inputNamePrefix + "Vertical";
        attractInputName = inputNamePrefix + "Attract";
    }

    private void OnDisable()
    {
        sheepAttractor.SetActive(false);
        input = Vector2.zero;
        finalVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;
    }

    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
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

    public void AddExternal(Vector2 amount)
    {
        finalVelocity += amount;
    }

    void FixedUpdate()
    {
        Vector2 inputVelocity;

        if (useAcceleration)
        {
            if (hasInput)
            {
                float max = maxSpeed * input.magnitude;
                inputVelocity = Vector2.ClampMagnitude(body.velocity + input * acceleration * Time.fixedDeltaTime, max);
            }
            else
            {
                inputVelocity = Vector2.Lerp(body.velocity, Vector2.zero, Time.fixedDeltaTime * damping);
            }
        }
        else
        {
            inputVelocity = input * maxSpeed;
        }

        finalVelocity = Vector2.Lerp(finalVelocity, inputVelocity, Time.fixedDeltaTime * externalDamping);
        body.velocity = finalVelocity;
    }
}