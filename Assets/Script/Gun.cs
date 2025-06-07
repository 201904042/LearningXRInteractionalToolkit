using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("발사 관련")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 500f;

    [Header("탄약 설정")]
    public int maxBullet = 10;
    private int currentBullet;
    private bool isLoaded = false;

    [Header("탄알집 관련")]
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

        // 탄알집 떨어뜨리기
        GameObject mag = Instantiate(magazinePrefab, magAttachPoint.position, magAttachPoint.rotation);
        Rigidbody rb = mag.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        // 장전 해제
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
