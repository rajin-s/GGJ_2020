using UnityEngine;

public class ManualPlayer : MonoBehaviour
{
    private static KeyCode[] keys = {
        KeyCode.BackQuote,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
        KeyCode.Minus,
        KeyCode.Equals
    };

    [SerializeField] private int[] harmony;
    
    private Instrument instrument;

    private void Awake()
    {
        instrument = GetComponent<Instrument>();
    }

    private void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                instrument.PlayNote(i);
                for (int j = 0; j < harmony.Length; j++)
                {
                    instrument.PlayNote(i + harmony[j]);
                }
            }
        }
    }
}