using System.Collections;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private Transform playHead, start, end;
    [SerializeField] private float duration = 100;
    
    [SerializeField] private RhythmManager rhythmManager;

    private void Start()
    {
        playHead.position = start.position;
        Invoke("StartSong", 1);
    }

    private void StartSong()
    {
        rhythmManager.StartTime();
        StartCoroutine(MakeTimelineRoutine());
    }

    private IEnumerator MakeTimelineRoutine()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            if (t == 1.0f) t = 1.0f;

            playHead.position = Vector2.Lerp(start.position, end.position, t);
            yield return null;
        }

        rhythmManager.EndTime();
    }
}
