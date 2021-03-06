using UnityEngine;

public class PercussionInstrument : Instrument
{
    [Header("Percussion")]
    [SerializeField] PercussionKit kit;
    [SerializeField] private int bassNote = 1;

    public override void PlayNote(int step)
    {
        AudioClip clip = kit.GetClip(step);
        AudioSource source = GetOpenChannel();

        if (source != null)
        {
            source.clip = clip;
            source.Play();

            if (step == bassNote)
            {
                InstrumentPostprocessing.DoCallback(1);
            }
        }
        else
        {
            Debug.LogWarningFormat("{0} failed to play percussion note (no open channels)", name);
        }
    }
}