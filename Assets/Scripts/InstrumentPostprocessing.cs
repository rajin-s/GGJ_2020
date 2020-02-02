using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

[RequireComponent(typeof(PostProcessVolume))]
public class InstrumentPostprocessing : MonoBehaviour
{
    private static void DoNothing(float x) {}
    private static Action<float> callback = DoNothing;

    public static void DoCallback(float intensity)
    {
        callback(intensity);
    }

    [SerializeField] private AnimationCurve weightCurve;
    [SerializeField] private float decaySpeed = 10.0f;

    private float intensity;
    private PostProcessVolume volume;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void OnEnable()
    {
        callback += OnInstrument;
    }

    private void OnDisable()
    {
        callback -= OnInstrument;
    }

    private void OnInstrument(float newIntensity)
    {
        if (newIntensity > intensity)
        {
            intensity = newIntensity;
        }
    }

    private void LateUpdate()
    {
        intensity = Mathf.Lerp(intensity, 0, decaySpeed * Time.deltaTime);
        volume.weight = weightCurve.Evaluate(intensity);
    }
}