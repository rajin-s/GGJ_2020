using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class AudioPostProcessing : MonoBehaviour
{
    enum Source { Low, Mid, High };

    [SerializeField] private Source source;
    [SerializeField] private AnimationCurve weightCurve;
    private PostProcessVolume volume;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void LateUpdate()
    {
        float intensity = 0;
        switch (source)
        {
            case Source.Low:
                intensity = AudioManager.IntensityLow;
                break;
            case Source.Mid:
                intensity = AudioManager.IntensityMid;
                break;
            case Source.High:
                intensity = AudioManager.IntensityHigh;
                break;
        }

        volume.weight = weightCurve.Evaluate(intensity);
    }
}