using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class HarmonyPostprocessing : MonoBehaviour
{
    [SerializeField] private AnimationCurve weightCurve;
    [SerializeField] private float fadeSpeed = 3.0f;
    private PostProcessVolume volume;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void LateUpdate()
    {
        volume.weight = Mathf.Lerp(volume.weight, weightCurve.Evaluate(Level.GetHarmonyValue()), Time.deltaTime * fadeSpeed);
    }
}