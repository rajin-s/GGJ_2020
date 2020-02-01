using UnityEngine;

public class AutoPlayer : MonoBehaviour
{
    [SerializeField] private Rhythm.Subdivision rate;
    private Instrument instrument;

    private void Awake()
    {
        instrument = GetComponent<Instrument>();
    }

    private void OnEnable()
    {
        RhythmManager.AddHandler(rate, instrument.PlayNextNote);
    }
    private void OnDisable()
    {
        RhythmManager.RemoveHandler(rate, instrument.PlayNextNote);
    }
}