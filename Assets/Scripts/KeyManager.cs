using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private KeySequence keySequence;
    [SerializeField] private Rhythm.Subdivision subdivision;
    [SerializeField] private int subdivisionCount = 4;

    private Instrument[] instruments;

    private int subdivisions;

    private void ChangeKey()
    {
        Key key = keySequence.GetNextKey();

        for (int i = 0; i < instruments.Length; i++)
        {
            instruments[i].SetKey(key);
        }
    }

    private void OnSubdivision()
    {
        if (subdivisions % subdivisionCount == 0)
        {
            ChangeKey();
        }

        subdivisions += 1;
    }

    private void Awake()
    {
        instruments = GetComponentsInChildren<Instrument>();
    }

    private void OnEnable()
    {
        RhythmManager.AddHandler(subdivision, OnSubdivision);
    }

    private void OnDisable()
    {
        RhythmManager.RemoveHandler(subdivision, OnSubdivision);
    }
}