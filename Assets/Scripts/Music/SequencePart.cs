using UnityEngine;

[CreateAssetMenu(fileName = "SequencePart", menuName = "Music/Part/Sequence", order = 0)]
public class SequencePart : Part
{
    [SerializeField] private string sequenceString = "";
    private int[] sequence;

    private int current;

    private void Awake()
    {
        current = 0;
        
        var tokens = sequenceString.Split(' ');
        sequence = new int[tokens.Length];

        for (int i = 0; i < sequence.Length; i++)
        {
            int step;
            bool isStep = int.TryParse(tokens[i], out step);
            
            if (isStep)
            {
                sequence[i] = step;
            }
            else
            {
                sequence[i] = Instrument.noNote;
            }
        }
    }

    public override int GetNextNote()
    {
        int next = sequence[current];
        current = (current + 1) % sequence.Length;
        return next;
    }
}