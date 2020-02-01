using UnityEngine;
using System.Collections;

public class Pen : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private AnimationCurve sizeOverLifetime;
    [SerializeField] private AnimationCurve volumeOverFullness;
    [SerializeField] private AnimationCurve completenessOverFullness;
    
    [SerializeField] private int maxSheep = 10;
    private int sheepCount = 0;
    private float baseScale = 1;

    private Instrument instrument;

    public void Spawn()
    {
        gameObject.SetActive(true);
        StartCoroutine(MakeLifetimeRoutine());
    }

    private void UpdateInstrument()
    {
        float fullness = Mathf.Clamp01((float)sheepCount / maxSheep);
        
        float volume = volumeOverFullness.Evaluate(fullness);
        float completeness = completenessOverFullness.Evaluate(fullness);

        instrument.Volume = volume;
        instrument.completeness = completeness;
    }

    private void OnGainSheep(Sheep sheep)
    {
        sheepCount += sheep.Value;
        UpdateInstrument();
    }

    private void OnLoseSheep(Sheep sheep)
    {
        sheepCount -= sheep.Value;
        UpdateInstrument();
    }

    private IEnumerator MakeLifetimeRoutine()
    {
        float t = 0;
        while (t < 1.0f)
        {
            float size = sizeOverLifetime.Evaluate(t);
            transform.localScale = Vector3.one * size * baseScale;

            t += Time.deltaTime / lifetime;
            yield return null;
        }
    }

    private void Awake()
    {
        baseScale = transform.localScale.x;

        instrument = GetComponent<Instrument>();
        UpdateInstrument();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {
            OnGainSheep(sheep);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {
            OnLoseSheep(sheep);
        }
    }
}