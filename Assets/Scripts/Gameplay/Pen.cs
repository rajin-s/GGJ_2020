using UnityEngine;
using System.Collections;

public class Pen : MonoBehaviour
{
    [SerializeField] private string targetTag = "SheepA";
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private AnimationCurve sizeOverLifetime;
    [SerializeField] private AnimationCurve volumeOverFullness;
    [SerializeField] private AnimationCurve completenessOverFullness;

    [SerializeField] private int maxSheep = 10;
    private int sheepCount = 0;
    private float baseScale = 1;

    private Instrument instrument;

    [Header("Effects")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer border;
    [SerializeField] private CurveAsset borderScaleCurve;
    [SerializeField] private CurveAsset borderBrightnessCurve;
    [SerializeField] private Color borderFlashColor;

    private Color borderBaseColor;
    private float borderBaseScale;
    private float backgroundBaseAlpha;
    private float backgroundTargetAlpha;

    private Coroutine currentScaleRoutine;
    private Coroutine currentBrightnessRoutine;

    private void Awake()
    {
        baseScale = transform.localScale.x;

        instrument = GetComponent<Instrument>();

        borderBaseColor = border.color;
        borderBaseScale = border.transform.localScale.x;
        backgroundBaseAlpha = background.color.a;

        UpdateInstrument();
        Hide();
    }

    private void Update()
    {
        Color c = background.color;
        c.a = Mathf.Lerp(c.a, backgroundTargetAlpha, Time.deltaTime * 4.0f);
        background.color = c;
    }

    private void Hide()
    {
        transform.localScale = Vector3.zero;
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

    private void UpdateInstrument()
    {
        float fullness = Mathf.Clamp01((float)sheepCount / maxSheep);

        float volume = volumeOverFullness.Evaluate(fullness);
        float completeness = completenessOverFullness.Evaluate(fullness);

        instrument.Volume = volume;
        instrument.completeness = completeness;

        backgroundTargetAlpha = backgroundBaseAlpha * fullness;
    }

    private void OnGainSheep(Sheep sheep)
    {
        sheepCount += sheep.Value;
        UpdateInstrument();

        if (currentScaleRoutine != null)
        {
            StopCoroutine(currentScaleRoutine);
        }
        if (currentBrightnessRoutine != null)
        {
            StopCoroutine(currentBrightnessRoutine);
        }

        currentScaleRoutine = this.RunCurve(borderScaleCurve, (v) => { border.transform.localScale = v * Vector3.one * borderBaseScale; });
        currentBrightnessRoutine = this.RunCurve(borderBrightnessCurve, (v) => { border.color = Color.Lerp(borderBaseColor, borderFlashColor, v); });
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && sheep.gameObject.CompareTag(targetTag))
        {
            OnGainSheep(sheep);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && sheep.gameObject.CompareTag(targetTag))
        {
            OnLoseSheep(sheep);
        }
    }
}