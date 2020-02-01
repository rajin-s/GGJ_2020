using UnityEngine;
using System.Collections.Generic;

public class SheepAttractor : MonoBehaviour
{
    [SerializeField] private float force = 5;

    private bool active = false;

    [SerializeField]
    private List<Sheep> affectedSheep = new List<Sheep>();

    private void Activate()
    {
        active = true;
    } 
    private void Deactivate()
    {
        active = false;
    }

    public void SetActive(bool value)
    {
        if (value && !active)
        {
            Activate();
        }
        else if (!value && active)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && !affectedSheep.Contains(sheep))
        {
            affectedSheep.Add(sheep);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null && affectedSheep.Contains(sheep))
        {
            affectedSheep.Remove(sheep);
        }
    }

    private void UpdateSheep()
    {
        if (active)
        {
            foreach (Sheep sheep in affectedSheep)
            {
                sheep.PullTo(transform.position, force);
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateSheep();
    }
}