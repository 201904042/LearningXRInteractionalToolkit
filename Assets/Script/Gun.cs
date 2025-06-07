using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("�߻� ����")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 500f;

    [Header("ź�� ����")]
    public int maxBullet = 10;
    private int currentBullet;
    private bool isLoaded = false;

    [Header("ź���� ����")]
    public Transform magAttachPoint;
    public GameObject magazinePrefab;

    void Start()
    {
        currentBullet = maxBullet;
        isLoaded = true;
    }

    public void Shoot()
    {
        if (!isLoaded || currentBullet <= 0) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

        currentBullet--;

        if (currentBullet <= 0)
        {
            EjectLoad();
        }
    }

    public void EjectLoad()
    {
        if (!isLoaded) return;

        // ź���� ����߸���
        GameObject mag = Instantiate(magazinePrefab, magAttachPoint.position, magAttachPoint.rotation);
        Rigidbody rb = mag.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        // ���� ����
        isLoaded = false;
        currentBullet = 0;
    }

    public void Reload()
    {
        if (isLoaded) return;

        isLoaded = true;
        currentBullet = maxBullet;
    }
}
