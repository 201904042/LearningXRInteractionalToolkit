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

    [Header("디버그 설정")]
    public bool StartWithLoaded = false;

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
    }

    public void ActivateGun() { }
    public void DeactivateGun() { }


    [ContextMenu("shoot")]
    public void Shoot()
    {
        if (state != GunState.Ready || !HasAmmo())
        {
            return;
        }

        // 총알 생성
        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(bulletDamage, firePos.forward);
        StartCoroutine(DelayNextShot());

        // 슬라이더는 발사 시에도 자동으로 1발 소비
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
        Debug.Log("eject");
        if (usingMag == null) return;

        usingMag.GetComponent<Magazine>().SetPhysicsEnabled(true);
        usingMag.transform.SetParent(null);
        usingMag = null;
    }

    [ContextMenu("insert")]
    public void InsertMagazine()
    {
        Debug.Log("insert");
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

        //usingMag.GetComponent<Magazine>().SetPhysicsEnabled(false);
        //usingMag.transform.SetParent(magazineSocket.transform);
        Debug.Log("Inserted");
       
    }

    /// <summary>
    /// 슬라이더를 당기면 탄창에서 1발을 소비
    /// </summary>
    [ContextMenu("PullSlider")]
    public void PullSlider()
    {
        Debug.Log("PullSlider");
        if (usingMag == null) return;

        Magazine mag = usingMag.GetComponent<Magazine>();
        if (mag.GetBullets() > 0)
        {
            mag.UseBullet(); // 탄창에서 한 발 꺼냄
            state = GunState.Ready;
        }
        else
        {
            Debug.Log("탄약 없음");
        }
    }

    public bool HasMagazineAttached()
    {
        return usingMag != null;
    }

    public bool HasAmmo()
    {
        return usingMag != null && usingMag.GetComponent<Magazine>().GetBullets() > 0;
    }
}
