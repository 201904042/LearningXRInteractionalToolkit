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
    [SerializeField] private bool hasSlide = true;

    [Header("디버그 설정")]
    public bool StartWithLoaded = false;

    private void Start()
    {
        if (StartWithLoaded)
            StartCoroutine(DelayedAutoLoad());

        if(magazineSocket == null)
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

        // XR 상호작용으로 장전 시도
        //magazineSocket.interactionManager.SelectEnter(magazineSocket, magGrab);
        //XRGrabInteractable과 XRGunMagazineSocket을 각각 IXRSelectInteractable, IXRSelectInteractor로 전환이 필요함
        //명시적 캐스팅 사용
        magazineSocket.interactionManager.SelectEnter((IXRSelectInteractor)magazineSocket,(IXRSelectInteractable)magGrab);
    }


    [ContextMenu("Shoot")]
    public void Shoot()
    {
        if (state != GunState.Ready || !HasAmmo())
            return;

        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        if (bullet.TryGetComponent(out Bullet bulletScript))
            bulletScript.Init(bulletDamage, firePos.forward);

        StartCoroutine(DelayNextShot());
        PullSlider(); // 자동으로 1발 소비
    }

    private IEnumerator DelayNextShot()
    {
        state = GunState.Delay;
        yield return new WaitForSeconds(fireDelay);
        state = GunState.Ready;
    }

    [ContextMenu("Eject Magazine")]
    public void EjectMagazine(SelectExitEventArgs args)
    {
        
        Debug.Log("탄창 분리됨");
        return;
        usingMag = null;
    }


    [ContextMenu("Insert Magazine")]
    public void InsertMagazine(SelectEnterEventArgs args)
    {
        Debug.Log("탄창 장착됨");
        
        usingMag = args.interactableObject.transform.GetComponent<Magazine>();
        return;
        hasSlide = false;
    }

    [ContextMenu("Pull Slider")]
    public void PullSlider()
    {

        if (usingMag == null)
        {
            Debug.LogWarning("탄창이 없습니다.");
            return;
        }

        if (usingMag.BulletCount > 0)
        {
            hasSlide = true;
            usingMag.UseBullet();
            state = GunState.Ready;
            Debug.Log("슬라이더 당김 - 발사 준비됨");
        }
        else
        {
            Debug.Log("탄약 없음");
        }
    }

    public void ActivateGun() => state = GunState.Ready;
    public void DeactivateGun() => state = GunState.UnGrab;
    public bool HasMagazineAttached() => usingMag != null;
    public bool HasAmmo() => usingMag != null && usingMag.BulletCount > 0;
}
