using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    public string gunId;
    public string gunName;
    public float damage;
    public float fireRate;
    public float bulletForce;
    public int maxBullet;
    public Enums.GunType type;
    public GameObject bulletPrefab;
}
