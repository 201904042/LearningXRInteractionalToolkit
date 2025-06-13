using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Enums;

public class GunObject : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public XRSocketInteractor magazineSocket;

    [Header("GunData")]
    public GunData gunData;

    [Header("총기 설정")]
    public Transform firePos;

    [Header("탄약 설정")]
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;

    [Header("탄알집")]
    public Transform magAttachPoint;
    [SerializeField] private GameObject usingMag;
    [SerializeField] private GunState state;

    [Header("약실 상태")]
    //총을 발사하는 최종판단 -> 탄창이 빠져도 약실에 총알이 있다면 발사가 가능함
    [SerializeField] private bool isBulletChambered = false;

    [Header("디버그 설정")]
    public bool StartWithLoaded = false;

    private void Awake()
    {
    }

    private void Start()
    {
        if (StartWithLoaded)
        {
            StartCoroutine(DelayedAutoLoad());
        }
    }

    private IEnumerator DelayedAutoLoad()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject magObj = Instantiate(gunData.magPrefab, magAttachPoint.position, magAttachPoint.rotation);
        Magazine mag = magObj.GetComponent<Magazine>();
        XRGrabInteractable magGrab = magObj.GetComponent<XRGrabInteractable>();

        magGrab.interactionManager.SelectEnter(
            (IXRSelectInteractor)magazineSocket,
            (IXRSelectInteractable)magGrab
        );

        InsertMagazine();
        PullSlider();
    }

    public void ActivateGun() { }

    public void DeactivateGun() { }

    [ContextMenu("shoot")]
    public void Shoot()
    {
        if (state != GunState.Ready || !isBulletChambered)
        {
            return;
        }

        //총알 생성
        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(bulletDamage, firePos.forward);
        StartCoroutine(DelayNextShot());

        //발사시 자동 슬라이더 당겨짐
        PullSlider();
    }

    private IEnumerator DelayNextShot()
    {
        state = GunState.Delay;
        yield return new WaitForSeconds(fireDelay);
        state = GunState.Ready;
    }

    [ContextMenu("eject")]
    public void EjectMagazine()
    {
        if (usingMag == null) return;

        usingMag.GetComponent<Magazine>().SetPhysicsEnabled(true);
        usingMag.transform.SetParent(null);
        usingMag = null;
    }

    [ContextMenu("insert")]
    public void InsertMagazine()
    {
        // 이미 탄창이 끼워져 있으면 새 탄창은 무시
        if (usingMag != null)
        {
            Debug.Log("이미 탄창이 끼워져 있습니다.");
            return;
        }

        var selected = magazineSocket.GetOldestInteractableSelected();
        if (selected == null)
        {
            Debug.Log("소켓에 착용된 탄창을 찾지 못함");
            return;
        }

        usingMag = selected.transform.gameObject;
        if (usingMag == null)
        {
            Debug.Log("매거진을 찾지못함");
            return;
        }

        Debug.Log("Inserted");
        usingMag.GetComponent<Magazine>().SetPhysicsEnabled(false);
        usingMag.transform.SetParent(magazineSocket.transform);
    }

    /// <summary>
    /// 슬라이더를 당겼을 때 호출 (직접 입력으로도 가능)
    /// </summary>
    [ContextMenu("PullSlider")]
    public void PullSlider()
    {
        if (usingMag == null)
        {
            isBulletChambered = false; 
            return;
        }

        Magazine mag = usingMag.GetComponent<Magazine>();
        if (mag.GetBullets() > 0)
        {
            mag.UseBullet(); // 탄창에서 한 발 꺼냄
            isBulletChambered = true;
            state = GunState.Ready;
        }
        else
        {
            isBulletChambered = false;
        }
    }

    public bool HasMagazineAttached()
    {
        return usingMag != null;
    }


}
