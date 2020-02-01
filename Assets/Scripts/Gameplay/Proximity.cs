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

    [SerializeField] private float baseScale = 1;
    [SerializeField] private float currentScale;
    [SerializeField] private float targetDistance = 999;

    private List<Transform> targets = new List<Transform>();

    private Instrument instrument;

    private void Awake()
    {
        baseScale = transform.localScale.x;
        instrument = GetComponent<Instrument>();
        // gameObject.SetActive(false);
        Spawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(targetTag) && !targets.Contains(other.transform))
        {
            targets.Add(other.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(targetTag) && targets.Contains(other.transform))
        {
            targets.Remove(other.transform);
        }
    }

    private void Update()
    {
        float minDistance = 9999;
        foreach (Transform target in targets)
        {
            float distance = ((Vector2)target.position - (Vector2)transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        targetDistance = minDistance;
        UpdateInstrument();
    }

    private void UpdateInstrument()
    {
        float value;
        if (currentScale == 0)
        {
            value = 0;
        }
        else
        {
            float radius = currentScale / 2 - margin;
            value = Mathf.InverseLerp(minDistanceScale * radius, radius, targetDistance);
            value = Mathf.Clamp01(value);
        }

        instrument.Volume = volumeOverDistance.Evaluate(value);
        instrument.completeness = completenessOverDistance.Evaluate(value);
    }

    public void Spawn()
    {
        StartCoroutine(MakeLifetimeRoutine());
    }

    private IEnumerator MakeLifetimeRoutine()
    {
        float t = 0;
        while (t < 1.0f)
        {
            currentScale = sizeOverLifetime.Evaluate(t) * baseScale;
            transform.localScale = Vector3.one * currentScale;

            t += Time.deltaTime / lifetime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}