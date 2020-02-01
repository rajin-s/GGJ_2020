using UnityEngine;

[CreateAssetMenu(fileName = "RandomPart", menuName = "Music/Part/Random", order = 0)]
public class RandomPart : Part
{
    [SerializeField] private int[] options;

    public override int GetNextNote()
    {
        int index = Random.Range(0, options.Length);
        return options[index];
    }
}