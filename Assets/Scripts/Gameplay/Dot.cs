using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private float fadeDuration = 0.25f;
    
    private DotSequence sequence;
    private SpriteRenderer sprite;
    
    private bool alive;
    private bool fading;

    private void Awake()
    {
        sequence = GetComponentInParent<DotSequence>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        alive = true;
    }

    private void Collect()
    {
        sequence.OnDotCollected();
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
        StartCoroutine(MakeSpawnRoutine());
    }
    public void Despawn()
    {
        fading = true;
        if (alive)
        {
            StartCoroutine(MakeDespawnRoutine());
        }
    }

    private IEnumerator MakeSpawnRoutine()
    {
        float t = 0;

        Color spriteColor = sprite.color;
        float startAlpha = spriteColor.a;

        while (t < 1.0f)
        {
            spriteColor.a = startAlpha * t;
            sprite.color = spriteColor;
            
            t += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
    private IEnumerator MakeDespawnRoutine()
    {
        float t = 0;

        Color spriteColor = sprite.color;
        float startAlpha = spriteColor.a;

        while (t < 1.0f)
        {
            spriteColor.a = startAlpha * (1f - t);
            sprite.color = spriteColor;
            
            t += Time.deltaTime / fadeDuration;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}