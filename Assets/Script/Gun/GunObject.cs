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

    [Header("�ѱ� ����")]
    public Transform firePos;

    [Header("ź�� ����")]
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;

    [Header("ź����")]
    public Transform magAttachPoint;
    [SerializeField] private Magazine usingMag;
    [SerializeField] private GunState state;
    [SerializeField] private bool hasSlide = true;

    [Header("����� ����")]
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
            Debug.LogWarning("źâ �����տ� Magazine �Ǵ� XRGrabInteractable�� �����ϴ�.");
            yield break;
        }

        // XR ��ȣ�ۿ����� ���� �õ�
        //magazineSocket.interactionManager.SelectEnter(magazineSocket, magGrab);
        //XRGrabInteractable�� XRGunMagazineSocket�� ���� IXRSelectInteractable, IXRSelectInteractor�� ��ȯ�� �ʿ���
        //����� ĳ���� ���
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
        PullSlider(); // �ڵ����� 1�� �Һ�
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
        
        Debug.Log("źâ �и���");
        return;
        usingMag = null;
    }


    [ContextMenu("Insert Magazine")]
    public void InsertMagazine(SelectEnterEventArgs args)
    {
        Debug.Log("źâ ������");
        
        usingMag = args.interactableObject.transform.GetComponent<Magazine>();
        return;
        hasSlide = false;
    }

    [ContextMenu("Pull Slider")]
    public void PullSlider()
    {

        if (usingMag == null)
        {
            Debug.LogWarning("źâ�� �����ϴ�.");
            return;
        }

        if (usingMag.BulletCount > 0)
        {
            hasSlide = true;
            usingMag.UseBullet();
            state = GunState.Ready;
            Debug.Log("�����̴� ��� - �߻� �غ��");
        }
        else
        {
            Debug.Log("ź�� ����");
        }
    }

    public void ActivateGun() => state = GunState.Ready;
    public void DeactivateGun() => state = GunState.UnGrab;
    public bool HasMagazineAttached() => usingMag != null;
    public bool HasAmmo() => usingMag != null && usingMag.BulletCount > 0;
}
