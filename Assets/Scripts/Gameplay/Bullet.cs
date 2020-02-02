using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool IsAvailable { get; private set; }
    
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private string targetTag = "";
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private float knockbackScale = 15f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private SpriteRenderer coreSprite;

    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioSource hitSFX;
    
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private AudioSource fireSFX;

    private Vector2 velocity;

    public void Fire(Vector2 origin, Vector2 velocity)
    {
        IsAvailable = false;
        transform.position = origin;
        this.velocity = velocity;

        coreSprite.enabled = true;
        trail.Play();

        fireSFX.Play();
        fireEffect.Play();

        Invoke("Deactivate", lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAvailable) return;

        if (targetTag.Length == 0 || other.gameObject.CompareTag(targetTag))
        {
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Damage(damage, (other.transform.position - transform.position).normalized * knockbackScale);

                hitSFX.Play();
                hitEffect.Play();

                Deactivate();
            }
        }
    }

    private void Deactivate()
    {
        if (IsAvailable) return;

        CancelInvoke();
        IsAvailable = true;

        coreSprite.enabled = false;
        trail.Stop();
    }

    private void Update()
    {
        if (IsAvailable) return;

        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}