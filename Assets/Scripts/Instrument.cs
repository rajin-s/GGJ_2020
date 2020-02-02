using UnityEngine;

public class Instrument : MonoBehaviour
{
    public const int noNote = 404;

    [Header("Part")]
    [SerializeField] private Key key;
    [SerializeField] private Part part;
    [SerializeField] private int octave = 3;

    [Header("Clip")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private float clipPitch = 440;
    [SerializeField, Range(0,1)] private float volume = 0.5f;

    [Header("Config")]
    [SerializeField] private int channelCount = 32;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup outputMixerGroup;

    [Header("Gameplay")]
    [SerializeField] private int harmonyValue = 1;

    private AudioSource[] channels;

    public float Volume
    {
        get { return volume; }
        set
        {
            volume = value;
            if (channels == null) return;
            
            for (int i = 0; i < channelCount; i++)
            {
                channels[i].volume = volume;
            }
        }
    }
    [Range(0,1)] public float completeness = 1.0f;

    private void CreateChannels()
    {
        channels = new AudioSource[channelCount];
        for (int i = 0; i < channelCount; i++)
        {
            channels[i] = gameObject.AddComponent<AudioSource>();
            channels[i].playOnAwake = false;
            channels[i].loop = false;
            channels[i].clip = clip;
            channels[i].outputAudioMixerGroup = outputMixerGroup;
            channels[i].volume = volume;
        }
    }

    private int channelCheckStart = 0;
    protected AudioSource GetOpenChannel()
    {
        for (int i = 0; i < channelCount; i++)
        {
            int index = (channelCheckStart + i) % channels.Length;

            if (!channels[index].isPlaying)
            {
                channelCheckStart = index;
                return channels[index];
            }
        }
        return null;
    }

    public virtual void PlayNote(int step)
    {
        float pitch = key.tonality.GetPitch(key.root, octave, step, clipPitch);
        // Debug.LogFormat("Play note: step={0}, pitch={1}", step, pitch);

        AudioSource source = GetOpenChannel();

        Level.AddHarmony(harmonyValue);

        if (source != null)
        {
            source.pitch = pitch;
            source.Play();
        }
        else
        {
            Debug.LogWarningFormat("{0} failed to play note (no open channels)", name);
        }
    }

    public virtual void SetKey(Key newKey)
    {
        key = newKey;
    }

    public void PlayNextNote()
    {
        int next = part.GetNextNote();
        if (next != noNote)
        {
            if (Random.value <= completeness)
            {
                PlayNote(next);
            }
        }
    }

    private void Awake()
    {
        CreateChannels();
        part = Instantiate(part);
    }
}