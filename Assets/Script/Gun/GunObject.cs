using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Enums;

public class GunObject : MonoBehaviour
{
    [Header("Interaction")]
    public XRGrabInteractable grabInteractable;
    public XRGunMagazineSocket magazineSocket;

    [Header("Gun Data")]
    public GunData gunData;

    [Header("총기 설정")]
    public Transform firePos;

    [Header("탄약 설정")]
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;

    [Header("탄알집")]
    public Transform magAttachPoint;
    [SerializeField] private Magazine usingMag;
    [SerializeField] private GunState state;

    [Header("디버그 설정")]
    public bool StartWithLoaded = false;
    public bool DebugMod = false;

    private void Start()
    {
        if (StartWithLoaded)
            StartCoroutine(DelayedAutoLoad());

        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (magazineSocket == null)
            magazineSocket = GetComponentInChildren<XRGunMagazineSocket>();

        magazineSocket.selectEntered.AddListener(InsertMagazine);
        magazineSocket.selectExited.AddListener(EjectMagazine);
    }

    private IEnumerator DelayedAutoLoad()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject magObj = Instantiate(gunData.magPrefab, magAttachPoint.position, magAttachPoint.rotation);
        if (!magObj.TryGetComponent(out Magazine mag) || !magObj.TryGetComponent(out XRGrabInteractable magGrab))
        {
            Debug.LogWarning("탄창 프리팹에 Magazine 또는 XRGrabInteractable이 없습니다.");
            yield break;
        }

        magazineSocket.interactionManager.SelectEnter(
            (IXRSelectInteractor)magazineSocket,
            (IXRSelectInteractable)magGrab
        );
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        if (!DebugMod && (state != GunState.Ready || !HasAmmo()))
            return;

        Vector3 origin = firePos.position;
        Vector3 direction = firePos.forward;

        // Raycast로 충돌 체크
        if (Physics.Raycast(origin, direction, out RaycastHit hit, gunData.GetBulletDistance(), ~0))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            // 데미지 처리
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.ApplyDamage(bulletDamage);
            }

            // 총알 비주얼 처리
            SpawnBulletVisual(origin, hit.point);
        }
        else
        {
            // 명중 대상 없으면 최대 거리로 비주얼 처리
            SpawnBulletVisual(origin, origin + direction * gunData.GetBulletDistance());
        }

        StartCoroutine(DelayNextShot());
        PullSlider();
    }

    // 비주얼용 총알 생성
    private void SpawnBulletVisual(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;

        Quaternion rotation;
        if (direction != Vector3.zero)
            rotation = Quaternion.LookRotation(direction);
        else
            rotation = Quaternion.identity; // 기본 회전값

        GameObject bullet = Instantiate(gunData.bulletPrefab, start, rotation);

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.InitVisual(start, end);
        }
    }


    private IEnumerator DelayNextShot()
    {
        state = GunState.Delay;
        yield return new WaitForSeconds(fireDelay);
        state = HasAmmo() ? GunState.Ready : GunState.NoAmmo;
    }

    [ContextMenu("Eject Magazine")]
    public void EjectMagazine(SelectExitEventArgs args)
    {
        Debug.Log("탄창 분리됨");
        usingMag.transform.SetParent(null);
        usingMag = null;
        state = GunState.NoMag;
    }

    [ContextMenu("Insert Magazine")]
    public void InsertMagazine(SelectEnterEventArgs args)
    {
        Debug.Log("탄창 장착됨");
        usingMag = args.interactableObject.transform.GetComponent<Magazine>();
        usingMag.transform.SetParent(magAttachPoint);
        state = GunState.NoSlide;
    }

    [ContextMenu("Pull Slider")]
    public void PullSlider()
    {
        if (usingMag == null)
        {
            Debug.LogWarning("탄창이 없습니다.");
            state = GunState.NoMag;
            return;
        }

        if (usingMag.BulletCount > 0)
        {
            usingMag.UseBullet();
            state = GunState.Ready;
        }
        else
        {
            state = GunState.NoAmmo;
        }
    }

    public void ActivateGun()
    {
        if(state== GunState.Ready || state == GunState.Delay)
        {
            state = GunState.Ready;
            return;
        }

        if (usingMag == null)
        {
            state = GunState.NoMag;
            return;
        }

        if (usingMag.BulletCount == 0)
            state = GunState.NoAmmo;
        else
            state = GunState.NoSlide;
    }

    public void DeactivateGun() => state = GunState.UnGrab;

    public bool HasMagazineAttached() => usingMag != null;

    public bool HasAmmo() => usingMag != null && usingMag.BulletCount > 0;
}
