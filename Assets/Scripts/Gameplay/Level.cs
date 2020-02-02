using UnityEngine;

public class Level : MonoBehaviour
{
    public static Rect Area { get; private set; }

    [SerializeField] private Camera levelCamera;
    [SerializeField, Range(0, 100)] private int startHarmonyPercent = 25;

    private void Awake()
    {
        Rect area = new Rect();
        area.position = levelCamera.transform.position;
        area.width = levelCamera.orthographicSize * levelCamera.aspect;
        area.height = levelCamera.orthographicSize;
        Area = area;
    }

    private void OnEnable()
    {
        singleton = this;
    }

    private void OnDisable()
    {
        singleton = null;
        harmony = harmonySteps * startHarmonyPercent / 100;
    }

    [SerializeField] private int harmonySteps = 100;
    [SerializeField] private int harmony;

    private static Level singleton;
    public static void AddHarmony(int amount)
    {
        if (singleton != null)
        {
            singleton.harmony = Mathf.Clamp(singleton.harmony + amount, 0, singleton.harmonySteps);
        }
    }

    public static float GetHarmonyValue()
    {
        if (singleton != null)
        {
            return (float)singleton.harmony / singleton.harmonySteps;
        }
        else
        {
            return 1;
        }
    }
}