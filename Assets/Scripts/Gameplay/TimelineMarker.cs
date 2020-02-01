using UnityEngine;

public class TimelineMarker : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private string methodOnEnter = "Spawn";
    [SerializeField] private string methodOnExit = "Despawn";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (methodOnEnter == "") return;
        target.SendMessage(methodOnEnter, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (methodOnExit == "") return;
        target.SendMessage(methodOnExit, SendMessageOptions.DontRequireReceiver);
    }
}
