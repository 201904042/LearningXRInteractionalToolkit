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

    [Header("�ѱ� ����")]
    public Transform firePos;

    [Header("ź�� ����")]
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;

    [Header("ź����")]
    public Transform magAttachPoint;
    [SerializeField] private GameObject usingMag;
    [SerializeField] private GunState state;

    [Header("����� ����")]
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

        // �Ѿ� ����
        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(bulletDamage, firePos.forward);
        StartCoroutine(DelayNextShot());

        // �����̴��� �߻� �ÿ��� �ڵ����� 1�� �Һ�
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
            Debug.Log("�̹� źâ�� ������ �ֽ��ϴ�.");
            return;
        }

        var selected = magazineSocket.GetOldestInteractableSelected();
        if (selected == null)
        {
            Debug.Log("���Ͽ� ����� źâ�� ã�� ����");
            return;
        }

        usingMag = selected.transform.gameObject;
        if (usingMag == null)
        {
            Debug.Log("�Ű����� ã������");
            return;
        }

        //usingMag.GetComponent<Magazine>().SetPhysicsEnabled(false);
        //usingMag.transform.SetParent(magazineSocket.transform);
        Debug.Log("Inserted");
       
    }

    /// <summary>
    /// �����̴��� ���� źâ���� 1���� �Һ�
    /// </summary>
    [ContextMenu("PullSlider")]
    public void PullSlider()
    {
        Debug.Log("PullSlider");
        if (usingMag == null) return;

        Magazine mag = usingMag.GetComponent<Magazine>();
        if (mag.GetBullets() > 0)
        {
            mag.UseBullet(); // źâ���� �� �� ����
            state = GunState.Ready;
        }
        else
        {
            Debug.Log("ź�� ����");
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
