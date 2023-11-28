using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;

    public enum ShootingPattern
    {
        SingleShot,
        TripleShot,
        // Agrega más tipos de disparo según sea necesario
    }

    public void Shoot(ShootingPattern pattern)
    {
        switch (pattern)
        {
            case ShootingPattern.SingleShot:
                GameObject bullet = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(-bullet.transform.up * bullet.GetComponent<Projectile>().speed, ForceMode2D.Impulse);
                Destroy(bullet, 2f);
                break;

            case ShootingPattern.TripleShot:
                Quaternion[] bulletRotations = new Quaternion[3];
                bulletRotations[0] = firePoint.transform.rotation;
                bulletRotations[1] = Quaternion.Euler(0, 0, firePoint.transform.rotation.eulerAngles.z + 15f);
                bulletRotations[2] = Quaternion.Euler(0, 0, firePoint.transform.rotation.eulerAngles.z - 15f);

                for (int i = 0; i < 3; i++)
                {
                    GameObject bullet2 = Instantiate(projectile, firePoint.transform.position, bulletRotations[i]);
                    Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                    rb2.AddForce(-bullet2.transform.up * bullet2.GetComponent<Projectile>().speed, ForceMode2D.Impulse);
                    Destroy(bullet2, 2f);
                }
                break;

                // Agrega más casos según los tipos de disparo que necesites
        }
    }
}
