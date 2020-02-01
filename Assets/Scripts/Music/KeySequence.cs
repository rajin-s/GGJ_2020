using UnityEngine;

[CreateAssetMenu(fileName = "KeySequence", menuName = "Music/KeySequence", order = 0)]
public class KeySequence : ScriptableObject
{
    [System.Serializable]
    private struct Phrase
    {
        public Key[] keys;
    }

    [SerializeField] private Phrase[] phrases;

    private int currentPhraseIndex;
    private int currentKeyIndex;

    public Key GetNextKey()
    {
        int currentPhraseLength = phrases[currentPhraseIndex].keys.Length;

        // Check that we're moving on from the last key in a phrase
        if (currentKeyIndex >= currentPhraseLength - 1)
        {
            int previousPhraseIndex = currentPhraseIndex;
            currentPhraseIndex = Random.Range(0, phrases.Length);
            
            if (currentPhraseIndex == previousPhraseIndex)
            {
                currentPhraseIndex = (currentPhraseIndex + 1) % phrases.Length;
            }
            
            currentKeyIndex = 0;
        }
        else
        {
            currentKeyIndex += 1;
        }

        return phrases[currentPhraseIndex].keys[currentKeyIndex];
    }
}