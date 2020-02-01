using UnityEngine;

[CreateAssetMenu(fileName = "LinearPart", menuName = "Music/Part/Linear", order = 0)]
public class LinearPart : Part
{
    [SerializeField] private int[] options;
    private int current = 0;
    private int octave = 0;
    int direction = 1;

    public override int GetNextNote()
    {
        if (octave > 1)
        {
            direction = -1;
        }
        else if (octave < 1)
        {
            direction = 1;
        }
        else if (Random.value < 0.2f)
        {
            direction = -direction;
        }
        
        current = current + direction;
        if (current < 0)
        {
            current += options.Length;
            octave -= 1;
        }
        else if (current >= options.Length)
        {
            current -= options.Length;
            octave += 1;
        }

        return options[current] + octave * 8;
    }
}