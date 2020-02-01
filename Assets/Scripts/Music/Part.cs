using UnityEngine;

public abstract class Part : ScriptableObject
{
    public virtual int GetNextNote() { return 0; }
}