using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float IntensityLow { get; private set; }
    public static float IntensityMid { get; private set; }
    public static float IntensityHigh { get; private set; }
    public static float IntensityAverage { get; private set; }

    [SerializeField, Range(0, 1)] private float lowDebug, midDebug, highDebug, averageDebug;

    [SerializeField] private int channel = 0;
    [SerializeField] private int sampleCount = 256;

    [SerializeField] private float riseSpeed = 10, fallSpeed = 4, normalizationSpeed = 1;
    [SerializeField] private Vector2 normalizationRange = new Vector2(0, 1);

    [SerializeField] private Vector2 lowRange;
    [SerializeField] private Vector2 midRange;
    [SerializeField] private Vector2 highRange;
    [SerializeField] private Vector2 averageRange;

    private float lowMax = 1, midMax = 1, highMax = 1, averageMax = 1;

    //private AudioSource source;
    private float[] samples;

    private void Awake()
    {
        samples = new float[sampleCount];
    }

    private void Update()
    {
        UpdateSamples();
    }

    private void UpdateSamples()
    {
        // Sample audio source data
        AudioListener.GetSpectrumData(samples, channel, FFTWindow.Blackman);

        float low = 0, mid = 0, high = 0, average = 0;
        int lowCount = 1, midCount = 1, highCount = 1, averageCount = 1;

        // Get band data
        for (int i = (int)(lowRange.x); i < (int)(lowRange.y); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            low += samples[i];
        }
        for (int i = (int)(midRange.x); i < (int)(midRange.y); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            mid += samples[i];
        }
        for (int i = (int)(highRange.x); i < (int)(highRange.y); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            high += samples[i];
        }
        for (int i = (int)(averageRange.x); i < (int)(averageRange.y); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            average += samples[i];
        }

        // Average band data
        low /= lowCount;
        mid /= midCount;
        high /= highCount;
        average /= averageCount;

        // Naturally lower normalization scale
        lowMax = Mathf.Lerp(lowMax, normalizationRange.x, Time.deltaTime * normalizationSpeed);
        midMax = Mathf.Lerp(midMax, normalizationRange.x, Time.deltaTime * normalizationSpeed);
        highMax = Mathf.Lerp(highMax, normalizationRange.x, Time.deltaTime * normalizationSpeed);
        averageMax = Mathf.Lerp(averageMax, normalizationRange.x, Time.deltaTime * normalizationSpeed);

        // Reset normalization scale if peaking
        if (low > lowMax) { lowMax = low; }
        if (mid > midMax) { midMax = mid; }
        if (high > highMax) { highMax = high; }
        if (average > averageMax) { averageMax = average; }
        
        // Clamp values
        if (lowMax > normalizationRange.y) { lowMax = normalizationRange.y; }
        if (midMax > normalizationRange.y) { midMax = normalizationRange.y; }
        if (highMax > normalizationRange.y) { highMax = normalizationRange.y; }
        if (averageMax > normalizationRange.y) { averageMax = normalizationRange.y; }

        // Normalize
        low /= lowMax;
        mid /= midMax;
        high /= highMax;
        average /= averageMax;

        // Adjust public values smoothly
        IntensityLow = Mathf.Lerp(IntensityLow, low, Time.deltaTime * (low > IntensityLow ? riseSpeed : fallSpeed));
        IntensityMid = Mathf.Lerp(IntensityMid, mid, Time.deltaTime * (mid > IntensityMid ? riseSpeed : fallSpeed));
        IntensityHigh = Mathf.Lerp(IntensityHigh, high, Time.deltaTime * (high > IntensityHigh ? riseSpeed : fallSpeed));
        IntensityAverage = Mathf.Lerp(IntensityAverage, average, Time.deltaTime * (average > IntensityAverage ? riseSpeed : fallSpeed));

        // Set debug slider values to help see response 
        lowDebug = IntensityLow;
        midDebug = IntensityMid;
        highDebug = IntensityHigh;
        averageDebug = IntensityAverage;
    }
}
