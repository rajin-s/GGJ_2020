using UnityEngine;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform shotOrigin;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10;

    private List<Bullet> bullets = new List<Bullet>();

    private Bullet GetNextBullet()
    {
        foreach (Bullet bullet in bullets)
        {
            if (bullet.IsAvailable)
            {
                return bullet;
            }
        }

        var newObject = Instantiate(bulletPrefab);
        var newBullet = newObject.GetComponent<Bullet>();
        bullets.Add(newBullet);
        return newBullet;
    }

    public void Fire()
    {
        Bullet bullet  = GetNextBullet();
        bullet.Fire(shotOrigin.position, shotOrigin.right * bulletSpeed);
    }
}