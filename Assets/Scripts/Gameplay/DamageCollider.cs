using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private float amount = 1;
    [SerializeField] private float knockbackScale = 1;
    [SerializeField] private string targetTag = "";

    private Vector2 GetKnockback(Vector2 point)
    {
        Vector2 direction = (point - (Vector2)transform.position).normalized;
        return direction * knockbackScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;

        if (targetTag.Length == 0 || other.gameObject.CompareTag(targetTag))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Damage(amount, GetKnockback(other.transform.position));
            }
        }
    }
}