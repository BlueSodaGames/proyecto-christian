using System.Collections;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Referencia al arma actual
    public Weapon currentWeapon;
    public static WeaponHandler Instance { get; private set; }
    public string actualState = "Idle";
    public TopDownPlayerMovement player;

    // Resto del código del WeaponHandler

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para iniciar el disparo
    public void StartShooting()
    {
        switch (currentWeapon.shootingPattern)
        {
            case Weapon.ShootingPattern.SingleShot:
                FireSingleShot();
                break;
            case Weapon.ShootingPattern.TripleShot:
                FireTripleShot();
                break;
            case Weapon.ShootingPattern.ConsecutiveShots:
                FireConsecutiveShots();
                break;
                // Agrega más casos para otros patrones
        }
    }

    private void FireSingleShot()
    {
        CinemachineShake.Instance.ShakeCamera(2f, .1f);
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, GameObject.FindGameObjectWithTag("FirePoint").transform.position, GameObject.FindGameObjectWithTag("FirePoint").transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(GameObject.FindGameObjectWithTag("FirePoint").transform.up * currentWeapon.bulletForce, ForceMode2D.Impulse);
        
        FindObjectOfType<AudioManager>().PlaySFX(currentWeapon.shootClips[Random.Range(0, currentWeapon.shootClips.Length)].name);
    }

    private void FireTripleShot()
    {
        CinemachineShake.Instance.ShakeCamera(2f, .1f);

        // Calculate rotation angles for the three bullets
        Quaternion[] bulletRotations = new Quaternion[3];
        bulletRotations[0] = GameObject.FindGameObjectWithTag("FirePoint").transform.rotation;
        bulletRotations[1] = Quaternion.Euler(0, 0, GameObject.FindGameObjectWithTag("FirePoint").transform.rotation.eulerAngles.z + 15f);
        bulletRotations[2] = Quaternion.Euler(0, 0, GameObject.FindGameObjectWithTag("FirePoint").transform.rotation.eulerAngles.z - 15f);

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(currentWeapon.bulletPrefab, GameObject.FindGameObjectWithTag("FirePoint").transform.position, bulletRotations[i]);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * currentWeapon.bulletForce, ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().PlaySFX(currentWeapon.shootClips[Random.Range(0, currentWeapon.shootClips.Length)].name);
            Destroy(bullet, currentWeapon.bulletDuration);
        }
    }

    // Método para detener el disparo (si es necesario)
    public void StopShooting()
    {
        // Detén las coroutines aquí si es necesario
        StopAllCoroutines();
    }

    

    private int consecutiveShotsCount = 0;

    private void FireConsecutiveShots()
    {
        CinemachineShake.Instance.ShakeCamera(2f, .1f);

        if (consecutiveShotsCount < 4) // Adjust the number of consecutive shots as needed
        {
            StartCoroutine(ConsecutiveShotsCoroutine());
        }
    }

    private IEnumerator ConsecutiveShotsCoroutine()
    {
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, GameObject.FindGameObjectWithTag("FirePoint").transform.position, GameObject.FindGameObjectWithTag("FirePoint").transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(GameObject.FindGameObjectWithTag("FirePoint").transform.up * currentWeapon.bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().PlaySFX(currentWeapon.shootClips[Random.Range(0, currentWeapon.shootClips.Length)].name);
        Destroy(bullet, currentWeapon.bulletDuration);

        consecutiveShotsCount++;

        yield return new WaitForSeconds(0.5f); // Adjust the delay between consecutive shots as needed

        if (consecutiveShotsCount >= 4) // Reset the count after a certain number of shots
        {
            consecutiveShotsCount = 0;
        }
    }

}
