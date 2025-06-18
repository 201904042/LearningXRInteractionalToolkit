using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Magazine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject colliderRoot;

    [Header("Settings")]
    [SerializeField] private int gunId;
    [SerializeField] private int bullets;
    [SerializeField] private bool autoInit = false;

    private bool isOnGrab = false;
    private Coroutine releaseCoroutine;

    public int GunId => gunId;
    public int BulletCount => bullets;
    public bool IsOnGrab => isOnGrab;
    public bool IsEmpty => bullets <= 0;

    private void Awake()
    {
        grabInteractable = grabInteractable ?? GetComponent<XRGrabInteractable>();
        rb = rb ?? GetComponent<Rigidbody>();

        if (autoInit)
            Init(0, 10);
    }

    public void Init(int id, int initialBullets)
    {
        gunId = id;
        bullets = initialBullets;
        EnableGrabbable(true);
    }

    public void OnGrab()
    {
        if (releaseCoroutine != null)
            StopCoroutine(releaseCoroutine);

        isOnGrab = true;
    }

    public void OnRelease()
    {
        if (releaseCoroutine != null)
            StopCoroutine(releaseCoroutine);

        releaseCoroutine = StartCoroutine(DelayedGrabReset());
    }

    private IEnumerator DelayedGrabReset()
    {
        yield return new WaitForSeconds(0.5f);
        isOnGrab = false;
        releaseCoroutine = null;
    }

    public void UseBullet()
    {
        if (bullets > 0)
            bullets--;
    }

    public void AddBullet(int count = 1)
    {
        bullets += count;
    }

    public void SetBulletCount(int count)
    {
        bullets = Mathf.Max(0, count);
    }

    public void EnableGrabbable(bool enable)
    {
        if (grabInteractable != null)
            grabInteractable.enabled = enable;
    }

    public void EnablePhysics(bool enable)
    {
        if (rb != null)
        {
            rb.isKinematic = !enable;
            rb.useGravity = enable;
        }

        if (colliderRoot != null)
        {
            colliderRoot.SetActive(enable);
        }

        Debug.Log($"[Magazine] Physics Enabled: {enable}");
    }
}
