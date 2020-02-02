using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3;
    [SerializeField] private float regenerationRate = 1;
    [SerializeField] private float invulnerabilityDuration = 0.25f;
    [SerializeField] private float respawnDelay = 3;

    [Header("Effects")]
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private AudioSource damageSFX;
    
    [SerializeField] private ParticleSystem dieEffect;
    [SerializeField] private AudioSource dieSFX;
    
    [SerializeField] private ParticleSystem respawnEffect;
    [SerializeField] private AudioSource respawnSFX;

    [SerializeField] private SpriteRenderer coreSprite;
    [SerializeField] private Gradient colorOverHealth;

    [SerializeField] private ParticleSystem trail;

    private PlayerController controller;

    private float currentHealth;
    private bool isDead = false;
    private bool canHit = true;

    private void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isDead)
        {
            currentHealth += Time.deltaTime * regenerationRate;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            float healthValue = currentHealth / maxHealth;
            coreSprite.color = colorOverHealth.Evaluate(healthValue);
        }

        // TEST
        if (Input.GetKeyDown(KeyCode.M))
        {
            Damage(1, Random.insideUnitCircle * 4);
        }
    }

    public void Damage(float amount, Vector2 knockback)
    {
        if (!isDead && canHit)
        {
            controller.AddExternal(knockback);
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                OnDeath();
            }
            else
            {
                OnDamage();
            }
        }
    }

    private void Respawn()
    {
        transform.position = Vector2.zero;
        currentHealth = maxHealth;

        OnRespawn();
    }

    private void OnDeath()
    {
        isDead = true;

        dieEffect.Play();
        dieSFX.Play();

        controller.enabled = false;
        coreSprite.enabled = false;
        trail.Stop();

        // transform.position = Vector2.up * 9999;

        Invoke("Respawn", respawnDelay);
    }

    private void OnDamage()
    {
        damageEffect.Play();
        damageSFX.Play();

        canHit = false;
        Invoke("ResetInvulnerability", invulnerabilityDuration);
    }

    private void OnRespawn()
    {
        isDead = false;
        
        respawnEffect.Play();
        respawnSFX.Play();

        controller.enabled = true;
        coreSprite.enabled = true;
        trail.Play();

        canHit = false;
        Invoke("ResetInvulnerability", invulnerabilityDuration);
    }

    private void ResetInvulnerability()
    {
        canHit = true;
    }
}