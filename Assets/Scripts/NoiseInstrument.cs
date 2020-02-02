using UnityEngine;

public class NoiseInstrument : Instrument
{
    [SerializeField] PercussionKit kit;

    public override void PlayNote(int step)
    {
        AudioClip clip = kit.GetRandomClip();
        AudioSource source = GetOpenChannel();

        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.LogWarningFormat("{0} failed to play percussion note (no open channels)", name);
        }
    }
}