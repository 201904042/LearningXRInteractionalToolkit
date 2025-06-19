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

    [Header("ÃÑ±â ¼³Á¤")]
    public Transform firePos;

    [Header("Åº¾à ¼³Á¤")]
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;

    [Header("Åº¾ËÁý")]
    public Transform magAttachPoint;
    [SerializeField] private Magazine usingMag;
    [SerializeField] private GunState state;

    [Header("µð¹ö±× ¼³Á¤")]
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
            Debug.LogWarning("ÅºÃ¢ ÇÁ¸®ÆÕ¿¡ Magazine ¶Ç´Â XRGrabInteractableÀÌ ¾ø½À´Ï´Ù.");
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

        GameObject bullet = Instantiate(gunData.bulletPrefab, firePos.position, firePos.rotation);
        if (bullet.TryGetComponent(out Bullet bulletScript))
            bulletScript.Init(bulletDamage, firePos.forward);

        StartCoroutine(DelayNextShot());
        PullSlider(); // ¹ß»ç ÈÄ ½½¶óÀÌ´õ Àç¼³Á¤
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
        Debug.Log("ÅºÃ¢ ºÐ¸®µÊ");
        usingMag.transform.SetParent(null);
        usingMag = null;
        state = GunState.NoMag;
    }

    [ContextMenu("Insert Magazine")]
    public void InsertMagazine(SelectEnterEventArgs args)
    {
        Debug.Log("ÅºÃ¢ ÀåÂøµÊ");
        usingMag = args.interactableObject.transform.GetComponent<Magazine>();
        usingMag.transform.SetParent(magAttachPoint);
        state = GunState.NoSlide;
    }

    [ContextMenu("Pull Slider")]
    public void PullSlider()
    {
        if (usingMag == null)
        {
            Debug.LogWarning("ÅºÃ¢ÀÌ ¾ø½À´Ï´Ù.");
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
