using System.Collections;
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
    private GameObject usingMag;
    [SerializeField] private GunState state;

    [Header("��� ����")]
    [SerializeField] private bool isBulletChambered = false;

    [Header("����� ����")]
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

        magazineSocket.startingSelectedInteractable = magGrab;

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

        //�Ѿ� ����
        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(bulletDamage, firePos.forward);
        StartCoroutine(DelayNextShot());

        //�߻�� �ڵ� �����̴� �����
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

        
        usingMag.transform.SetParent(null);
        usingMag = null;
    }

    [ContextMenu("insert")]
    public void InsertMagazine()
    {
        // �̹� źâ�� ������ ������ �� źâ�� ����
        if (usingMag != null)
        {
            Debug.Log("�̹� źâ�� ������ �ֽ��ϴ�.");
            return;
        }

        var selected = magazineSocket.GetOldestInteractableSelected();
        if (selected == null)
        {
            Debug.Log("���Ͽ� �ƹ��͵� �������� �ʾҽ��ϴ�.");
            return;
        }

        usingMag = selected.transform.gameObject;
        if (usingMag == null)
        {
            Debug.LogWarning("������ ������Ʈ�� Magazine ������Ʈ�� �����ϴ�.");
            return;
        }

        // ���� ��Ȱ��ȭ �� �θ� ����
        usingMag.GetComponent<Magazine>().SetPhysicsEnabled(false);
        usingMag.transform.SetParent(magazineSocket.transform);
    }

    /// <summary>
    /// �����̴��� ����� �� ȣ�� (���� �Է����ε� ����)
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
            mag.UseBullet(); // źâ���� �� �� ����
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
