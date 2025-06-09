using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static Enums;

public class Gun : MonoBehaviour
{
    [Header("InputAction")]
    public InputActionAsset inputActions;
    private InputAction FirstButton;
    private InputAction GripButton;
    private InputAction TriggerButton;

    [Header("GunData")]
    public GunData gunData;

    [Header("총기 설정")]
    public GunType gunType;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 1000f;
    public float bulletDamage = 10f;
    public float fireDelay = 0.1f;


    [Header("탄약 설정")]
    public int maxBullet = 10;
    [SerializeField] private int currentBullet;

    [Header("탄알집")]
    public GameObject viewMag;
    public Transform magAttachPoint;
    public GameObject magazinePrefab;
    private GameObject rejectedMagazine;
    [SerializeField] private bool isMagLoad = false;

    [SerializeField] private GunState state = GunState.NoMag;

    private void Start()
    {
        // 테스트용 초기화
        currentBullet = maxBullet;
        isMagLoad = true;
        state = GunState.Ready;

        ActivateInputSystem();

    }

    private void ActivateInputSystem()
    {
        FirstButton = inputActions.FindAction("XRI Left/PressPrimaryButton");
        if (FirstButton == null)
        {
            Debug.LogError("FirstButton 액션을 찾을 수 없습니다.");
        }

        TriggerButton = inputActions.FindAction("XRI Left/PressTriggerButton");
        if (FirstButton == null)
        {
            Debug.LogError("TriggerButton 액션을 찾을 수 없습니다.");
        }

        GripButton = inputActions.FindAction("XRI Left/PressGripButton");
        if (GripButton == null)
        {
            Debug.LogError("GripButton 액션을 찾을 수 없습니다.");
        }


        FirstButton.performed += OnXButtonPressed;
        FirstButton.Enable();
        TriggerButton.performed += OnTriggerPressed;
        TriggerButton.Enable();
        GripButton.performed += OnGripPressed;
        GripButton.Enable();
    }

    private void OnGripPressed(InputAction.CallbackContext context)
    {
        EjectMagazine();
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Shoot();
    }

    private void OnXButtonPressed(InputAction.CallbackContext context)
    {
        Reload();
    }

    [ContextMenu("shoot")]
    public void Shoot()
    {
        if (state != GunState.Ready) return;

        if (currentBullet <= 0)
        {
            state = GunState.NoAmmo;
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(bulletDamage, bulletForce, firePoint.forward);

        currentBullet--;

        if (currentBullet <= 0)
        {
            state = GunState.NoAmmo;
        }
        else
        {
            StartCoroutine(DelayNextShot());
        }
    }

    private IEnumerator DelayNextShot()
    {
        state = GunState.Delay;
        yield return new WaitForSeconds(fireDelay); // 예시용 발사 딜레이
        state = GunState.Ready;
    }

    [ContextMenu("eject")]
    public void EjectMagazine()
    {
        if (!isMagLoad) return;

        rejectedMagazine = Instantiate(magazinePrefab, magAttachPoint.position, magAttachPoint.rotation);

        viewMag.SetActive(false);

        isMagLoad = false;
        currentBullet = currentBullet == 0 ? 0 : 1;

        state = GunState.NoMag;
    }

    [ContextMenu("insert")]
    public void InsertMagazine()
    {
        viewMag.SetActive(true);
        currentBullet = maxBullet;
        isMagLoad = true;
        state = GunState.NoLoaded;
    }

    [ContextMenu("reload")]
    public void Reload() // VR에서 노리쇠 당겼을 때 호출
    {
        if (!isMagLoad || state == GunState.NoLoaded) return;
        state = GunState.Ready;
    }
}
