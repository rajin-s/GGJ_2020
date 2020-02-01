using UnityEngine;

public class Level : MonoBehaviour
{
    public static Rect Area { get; private set; }

    [SerializeField] private Camera levelCamera;

    private void Awake()
    {
        Rect area = new Rect();
        area.position = levelCamera.transform.position;
        area.width = levelCamera.orthographicSize * levelCamera.aspect;
        area.height = levelCamera.orthographicSize;
        Area = area;
    }
}