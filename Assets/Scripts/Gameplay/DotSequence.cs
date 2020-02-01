using UnityEngine;
using System.Collections;

public class DotSequence : MonoBehaviour
{
    [SerializeField] private Rhythm.Subdivision rate;
    [SerializeField] private float spawnDelay = 0.15f;

    private Dot[] dots;

    private Instrument instrument;
    int notesToPlay = 0;

    private void Awake()
    {
        instrument = GetComponent<Instrument>();
        dots = GetComponentsInChildren<Dot>();
        
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].gameObject.SetActive(false);
        }
    }

    // TEST
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         Spawn();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         Despawn();
    //     }
    // }

    private void OnEnable()
    {
        RhythmManager.AddHandler(rate, OnSubdivision);
    }

    private void OnDisable()
    {
        RhythmManager.RemoveHandler(rate, OnSubdivision);
    }

    private void OnSubdivision()
    {
        if (notesToPlay > 0)
        {
            notesToPlay -= 1;
            instrument.PlayNextNote();
        }
    }

    // Called from dot
    public void OnDotCollected()
    {
        notesToPlay += 1;
    }

    // Called from timeline
    public void Spawn()
    {
        StartCoroutine(MakeSpawnRoutine());
    }
    public void Despawn()
    {
        StartCoroutine(MakeDespawnRoutine());
    }

    private IEnumerator MakeSpawnRoutine()
    {
        var delay = new WaitForSeconds(spawnDelay);
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].Spawn();
            yield return delay;
        }
    }
    private IEnumerator MakeDespawnRoutine()
    {
        var delay = new WaitForSeconds(spawnDelay);
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].Despawn();
            yield return delay;
        }
    }
}