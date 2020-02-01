using UnityEngine;

[CreateAssetMenu(fileName = "PercussionKit", menuName = "Music/Percussion Kit", order = 0)]
public class PercussionKit : ScriptableObject
{
    [SerializeField] private AudioClip[] clips;

    public AudioClip GetClip(int step)
    {
        // 1-index steps to be consistent with music theory
        // (ie root = 1)
        step = step - 1;

        int stepCount = clips.Length;

        // Negative steps from the root
        if (step < 0)
        {
            // Find the offset from the start of the target octave (ie 8 note scale @ step -1 => 7)
            while (step < 0)
            {
                step += stepCount;
            }
            return clips[step];
        }
        else
        {
            return clips[step % stepCount];
        }
    }
}