using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [SerializeField] private string targetTag;

    [SerializeField] private CurveAsset fadeOutScaleCurve;
    [SerializeField] private CurveAsset fadeOutAlphaCurve;

    [SerializeField] private CurveAsset fadeInScaleCurve;
    [SerializeField] private CurveAsset fadeInAlphaCurve;

    [SerializeField] private Color pickupColor = Color.white;

    private DotSequence sequence;
    private SpriteRenderer sprite;

    private float baseScale = 1.0f;
    private Color baseColor;

    private bool alive;
    private bool fading;

    private void Awake()
    {
        sequence = GetComponentInParent<DotSequence>();
        sprite = GetComponent<SpriteRenderer>();

        baseScale = transform.localScale.x;
        baseColor = sprite.color;
    }

    private void OnEnable()
    {
        alive = true;
    }

    private void Collect()
    {
        sequence.OnDotCollected();
        
        baseColor = pickupColor;
        sprite.color = baseColor;

        GetComponentInChildren<ParticleSystem>().Play();

        Despawn();

        alive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alive || fading) return;

        if (other.gameObject.CompareTag(targetTag))
        {
            Collect();
        }
    }

    // Called from sequence spawn/despawn routine
    public void Spawn()
    {
        gameObject.SetActive(true);

        this.RunCurve(fadeInScaleCurve, (v) => { transform.localScale = Vector3.one * v * baseScale; });
        this.RunCurve(fadeInAlphaCurve, (v) => { Color c = baseColor; c.a *= v; sprite.color = c; });
    }
    public void Despawn()
    {
        fading = true;

        if (alive)
        {
            this.RunCurve(fadeOutScaleCurve, (v) => { transform.localScale = Vector3.one * v * baseScale; });
            this.RunCurve(fadeOutAlphaCurve, (v) => { Color c = baseColor; c.a *= v; sprite.color = c; });
        }
    }
}