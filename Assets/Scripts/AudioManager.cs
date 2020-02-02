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

    [SerializeField] private float maxRange = 24000;

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
        int lowCount = 0, midCount = 0, highCount = 0, averageCount = 0;

        // Get band data
        for (int i = (int)(lowRange.x / maxRange * sampleCount); i < (int)(lowRange.y / maxRange * sampleCount); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            low += samples[i];
            lowCount += 1;
        }
        for (int i = (int)(midRange.x / maxRange * sampleCount); i < (int)(midRange.y / maxRange * sampleCount); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            mid += samples[i];
            midCount += 1;
        }
        for (int i = (int)(highRange.x / maxRange * sampleCount); i < (int)(highRange.y / maxRange * sampleCount); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            high += samples[i];
            highCount += 1;
        }
        for (int i = (int)(averageRange.x / maxRange * sampleCount); i < (int)(averageRange.y / maxRange * sampleCount); i++)
        {
            //if (i < 0 || i >= sampleCount) break;
            average += samples[i];
            averageCount += 1;
        }

        // Average band data
        if (lowCount > 0) { low /= lowCount; }
        if (midCount > 0) { mid /= midCount; }
        if (highCount > 0) { high /= highCount; }
        if (averageCount > 0) { average /= averageCount; }

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
