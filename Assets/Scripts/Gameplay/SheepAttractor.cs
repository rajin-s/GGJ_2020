using UnityEngine;
using System.Collections.Generic;

public class SheepAttractor : MonoBehaviour
{
    [SerializeField] private float force = 5;
    [SerializeField] private string targetTag = "SheepA";

    [SerializeField] private AnimationCurve innerCircleScaleCurve;
    [SerializeField] private AnimationCurve outerCircleScaleCurve;

    private bool active = false;
    private List<Sheep> affectedSheep = new List<Sheep>();

    [Header("Effects")]
    [SerializeField] private Transform innerCircle;
    [SerializeField] private Transform outerCircle;
    [SerializeField] private float circleFadeDuration = 0.4f;

    [SerializeField] private LineRenderer[] lines;
    [SerializeField] private float lineJitter = 0.5f;
    [SerializeField] private float jitterSpeed = 20f;
    [SerializeField] private float jitterMaxDistance = 2.0f;
    [SerializeField] private float fadeInSpeed = 5.0f;
    [SerializeField] private float fadeOutSpeed = 2.0f;
    [SerializeField] private float lineWidth = 0.1f;

    private float radius;
    private float jitterOffset = 0.0f;
    private float circleTime = 0f;

    private void Awake()
    {
        radius = GetComponentInChildren<CircleCollider2D>().radius;
    }

    private void Activate()
    {
        active = true;
    }
    private void Deactivate()
    {
        active = false;
    }

    public void SetActive(bool value)
    {
        if (value && !active)
        {
            Activate();
        }
        else if (!value && active)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && other.gameObject.CompareTag(targetTag) && !affectedSheep.Contains(sheep))
        {
            affectedSheep.Add(sheep);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && affectedSheep.Contains(sheep))
        {
            affectedSheep.Remove(sheep);
        }
    }

    private void UpdateSheep()
    {
        if (active)
        {
            foreach (Sheep sheep in affectedSheep)
            {
                sheep.PullTo(transform.position, force);
            }
        }
    }

    private void UpdateCircles()
    {
        if (active)
        {
            circleTime += Time.deltaTime / circleFadeDuration;
            if (circleTime > 1.0f) circleTime = 1.0f;
        }
        else
        {
            circleTime -= Time.deltaTime / circleFadeDuration;
            if (circleTime < 0.0f) circleTime = 0.0f;
        }

        innerCircle.transform.localScale = innerCircleScaleCurve.Evaluate(circleTime) * Vector3.one * radius * 2;
        outerCircle.transform.localScale = outerCircleScaleCurve.Evaluate(circleTime) * Vector3.one * radius * 2;
    }

    private void UpdateLine(int index, Sheep target)
    {
        LineRenderer line = lines[index];

        if (target == null || !active)
        {
            Color c = line.endColor;
            c.a = Mathf.Lerp(c.a, -0.1f, Time.deltaTime * fadeOutSpeed);
            if (c.a < 0.01f)
            {
                c.a = 0.0f;
            }
            line.startWidth = Mathf.Lerp(line.startWidth, 0.0f, Time.deltaTime * fadeOutSpeed);
            line.endWidth = line.startWidth;

            line.endColor = c;
            c.a *= 0.5f;
            line.startColor = c;

        }
        else
        {
            float distance = (target.transform.position - transform.position).magnitude;
            if (distance > jitterMaxDistance)
            {
                UpdateLine(index, null);
                return;
            }

            float jitterAmount = Mathf.Clamp01((jitterMaxDistance - distance) / jitterMaxDistance);

            for (int i = 0; i < line.positionCount; i++)
            {
                float t = (float)i / (line.positionCount - 1);
                Vector2 position = Vector2.Lerp(transform.position, target.transform.position, t);

                if (t > 0 && t < 1)
                {
                    position.x += (Mathf.PerlinNoise(Time.time * jitterSpeed, jitterOffset + i * 900 + -index * 9000) * 2 - 1) * jitterAmount;
                    position.y += (Mathf.PerlinNoise(jitterOffset + i * 900 - index * 9000, Time.time * jitterSpeed) * 2 - 1) * jitterAmount;
                }

                line.SetPosition(i, position);
            }

            Color c = line.endColor;
            line.startWidth = jitterAmount * lineWidth;
            line.endWidth = line.startWidth;
            
            c.a = Mathf.Lerp(c.a, 1, Time.deltaTime * fadeInSpeed);
            line.endColor = c;
            c.a *= 0.5f;
            line.startColor = c;
        }

    }

    private void FixedUpdate()
    {
        UpdateSheep();
    }

    private void Update()
    {
        UpdateCircles();
        for (int i = 0; i < lines.Length; i++)
        {
            if (i < affectedSheep.Count)
            {
                UpdateLine(i, affectedSheep[i]);
            }
            else
            {
                UpdateLine(i, null);
            }
        }
    }
}