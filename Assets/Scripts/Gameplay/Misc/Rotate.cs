using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private bool active = true;

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    private void Update()
    {
        if (active)
        {
            transform.eulerAngles += Vector3.forward * speed * Time.deltaTime;
        }
    }
}