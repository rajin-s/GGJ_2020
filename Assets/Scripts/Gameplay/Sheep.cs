using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] private int value = 1;
    public int Value { get { return value; } }

    [SerializeField] private float minRespawnTime = 5, maxRespawnTime = 10;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(MakeRespawnRoutine());
    }

    public void PullTo(Vector2 position, float force)
    {
        Vector2 direction = (position - (Vector2)transform.position).normalized;
        body.AddForce(force * direction);
    }

    private void Fade(float t)
    {
        transform.localScale = t * Vector3.one;
    }

    private void MoveToNewPosition()
    {
        Vector2 newPosition;
        
        newPosition.x = Random.Range(Level.Area.xMin, Level.Area.xMax);
        newPosition.y = Random.Range(Level.Area.yMin, Level.Area.yMax);
        
        transform.position = newPosition;
    }

    private IEnumerator MakeRespawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minRespawnTime, maxRespawnTime));

            // Fade out
            float t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime / fadeDuration;
                if (t > 1.0f) t = 1.0f;

                Fade(1.01f - t);

                yield return null;
            }
            Fade(0);
            
            MoveToNewPosition();

            // Fade In
            t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime / fadeDuration;
                if (t > 1.0f) t = 1.0f;

                Fade(t);

                yield return null;
            }
        }
    }
}