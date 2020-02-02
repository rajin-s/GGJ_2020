using UnityEngine;
using System.Collections;

public class AutoCannon : MonoBehaviour
{
    private Cannon cannon;
    bool active = false;

    [SerializeField] private float shotDelay = 0.5f;

    private void Awake()
    {
        cannon = GetComponent<Cannon>();
    }

    public void Activate()
    {
        active = true;
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