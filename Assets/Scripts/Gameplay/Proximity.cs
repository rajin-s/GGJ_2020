using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Proximity : MonoBehaviour
{
    private const float margin = 0.1f;

    [SerializeField] private float lifetime = 10f;
    [SerializeField] private float minDistanceScale = 0.1f;
    [SerializeField] private string targetTag = "Player";

    [SerializeField] private AnimationCurve sizeOverLifetime;
    [SerializeField] private AnimationCurve volumeOverDistance;
    [SerializeField] private AnimationCurve completenessOverDistance;

    private float baseRadius = 10f;
    private float baseScale = 1;
    private float currentScale;
    private float targetDistance = 999;

    private bool active = false;

    private Instrument instrument;
    private Transform target = null;

    [Header("Effects")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private float lineJitter = 0.1f;
    [SerializeField] private float jitterSpeed = 20f;
    [SerializeField] private float fadeInSpeed = 5.0f;
    [SerializeField] private float fadeOutSpeed = 2.0f;

    private float jitterOffset = 0.0f;
    private float jitterAmount = 0.0f;

    private void Awake()
    {
        baseScale = transform.localScale.x;
        instrument = GetComponent<Instrument>();
        if (lifetime > 0){
            Hide();
        }
        UpdateInstrument();

        baseRadius = GetComponentInChildren<CircleCollider2D>().radius;
        jitterOffset = Random.Range(-9999f, 9999f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            target = other.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            target = null;
        }
    }

    private void Update()
    {
        UpdateInstrument();

        if (target == null)
        {
            targetDistance = 999;

            Color c = line.endColor;
            c.a = Mathf.Lerp(c.a, -0.1f, Time.deltaTime * fadeOutSpeed);
            if (c.a < 0.01f)
            {
                c.a = 0.0f;
            }
            line.endColor = c;

            c.a *= 0.75f;
            line.startColor = c;
            line.startColor = c;
        }
        else
        {
            targetDistance = ((Vector2)target.position - (Vector2)transform.position).magnitude;
            for (int i = 0; i < line.positionCount; i++)
            {
                float t = (float)i / (line.positionCount - 1);
                Vector2 position = Vector2.Lerp(transform.position, target.position, t);

                if (t > 0 && t < 1)
                {
                    position.x += (Mathf.PerlinNoise(Time.time * jitterSpeed, jitterOffset + i * 900) * 2 - 1) * jitterAmount;
                    position.y += (Mathf.PerlinNoise(jitterOffset + i * 900, Time.time * jitterSpeed) * 2 - 1) * jitterAmount;
                }

                line.SetPosition(i, position);
            }

            Color c = line.endColor;
            c.a = Mathf.Lerp(c.a, 1, Time.deltaTime * fadeInSpeed);
            line.endColor = c;
            c.a *= 0.75f;
            line.startColor = c;
        }
    }

    private void UpdateInstrument()
    {
        float value;
        if (currentScale == 0)
        {
            value = 1;
        }
        else
        {
            float radius = currentScale * baseRadius - margin;
            radius = Mathf.Max(radius, 0.1f);
            value = Mathf.InverseLerp(minDistanceScale * radius, radius, targetDistance);
            value = Mathf.Clamp01(value);
        }

        instrument.Volume = volumeOverDistance.Evaluate(value);
        instrument.completeness = completenessOverDistance.Evaluate(value);

        jitterAmount = instrument.Volume * lineJitter;
    }

    private void Hide()
    {
        transform.localScale = Vector3.zero;
        currentScale = 0;
        GetComponent<Collider2D>().enabled = false;
    }

    private void Show()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void Spawn()
    {
        Show();
        StartCoroutine(MakeLifetimeRoutine());
    }

    private IEnumerator MakeLifetimeRoutine()
    {
        active = true;
        float t = 0;
        while (t < 1.0f)
        {
            currentScale = sizeOverLifetime.Evaluate(t) * baseScale;
            transform.localScale = Vector3.one * currentScale;

            t += Time.deltaTime / lifetime;
            yield return null;
        }
        active = false;
        gameObject.SetActive(false);
    }
}