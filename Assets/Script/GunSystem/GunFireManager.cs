using UnityEngine;

public class GunFireManager : MonoBehaviour
{
    public Transform firePos;
    public GunData gunData;
    public GameObject decalPrefab;

    public void Fire()
    {
        Vector3 origin = firePos.position;
        Vector3 dir = firePos.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, gunData.GetBulletDistance(), ~0))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.ApplyDamage(gunData.GetDamage());
            }

            CreateDecal(hit);
        }
    }

    private void CreateDecal(RaycastHit hit)
    {
        if (decalPrefab == null) return;
        Quaternion rot = Quaternion.LookRotation(-hit.normal);
        Vector3 pos = hit.point + hit.normal * 0.01f;
        var decal = Instantiate(decalPrefab, pos, rot);
        Destroy(decal, 0.5f);
    }
}
