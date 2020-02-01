using UnityEngine;

[CreateAssetMenu(fileName = "Tonality", menuName = "Music/Tonality", order = 0)]
public class Tonality : ScriptableObject
{
    [SerializeField]
    private int[] chromaticSteps;

    public float GetPitch(Pitch.NoteName root, int octave, int step, double referencePitch)
    {
        // 1-index steps to be consistent with music theory
        // (ie root = 1)
        step = step - 1;

        int stepCount = chromaticSteps.Length;

        int chromaticOffset;

        // Negative steps from the root
        if (step < 0)
        {
            // Find the start of the target octave
            octave = octave - 1 + (step / stepCount);

            // Find the offset from the start of the target octave (ie 8 note scale @ step -1 => 7)
            while (step < 0)
            {
                step += stepCount;
            }
            chromaticOffset = chromaticSteps[step % stepCount];
        }
        else
        {
            octave += step / stepCount;
            chromaticOffset = chromaticSteps[step % stepCount];
        }

        float rootPitch = Pitch.GetRootPitchScale(root, octave, referencePitch);
        float pitchScale = Pitch.GetChromaticRatio(chromaticOffset);

        return rootPitch * pitchScale;
    }
}