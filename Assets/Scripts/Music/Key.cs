using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Music/Key", order = 0)]
public class Key : ScriptableObject
{
    [SerializeField] private Tonality _tonality;
    [SerializeField] private Pitch.NoteName _root;

    public Tonality tonality { get { return _tonality; } }
    public Pitch.NoteName root { get { return _root; } }
}