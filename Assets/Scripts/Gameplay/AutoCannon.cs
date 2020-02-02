using UnityEngine;
using System.Collections;

public class AutoCannon : MonoBehaviour
{
    private Cannon cannon;
    [SerializeField] bool active = false;
    [SerializeField] private float shotDelay = 0.5f;

    private void Awake()
    {
        cannon = GetComponent<Cannon>();
        if (active)
        {
            active = false;
            Activate();
        }
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            StartCoroutine(MakeFireRoutine());
        }
    }

    public void Deactivate()
    {
        active = false;
    }

    private IEnumerator MakeFireRoutine()
    {
        var delay = new WaitForSeconds(shotDelay);
        while (active)
        {
            cannon.Fire();
            yield return delay;
        }
    }
}