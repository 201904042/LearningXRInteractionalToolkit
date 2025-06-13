using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Magazine : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject cols;

    [SerializeField] private int gunId;
    [SerializeField] private int bullets;
    [SerializeField] private bool autoInit = false;
    [SerializeField] private bool isOnGrab = false;
    [SerializeField] private Coroutine release;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (autoInit)
            Init(0, 10);
    }

    public void OnGrab()
    {
        if(release != null)
            StopCoroutine(release);

        isOnGrab = true;
    }

    public void ReleaseGrab()
    {
        StartCoroutine(ReleaseOnSecond());
    }

    private IEnumerator ReleaseOnSecond()
    {
        yield return new WaitForSeconds(0.5f);
        isOnGrab = true;
    }

    public void Init(int id, int initialBullets)
    {
        gunId = id;
        bullets = initialBullets;
        SetGrabbable(true);
    }

    public void SetGrabbable(bool enabled)
    {
        if (grabInteractable != null)
            grabInteractable.enabled = enabled;
    }

    public void SetBullets(int count) => bullets = count;

    public int GetId() => gunId;

    public int GetBullets() => bullets;

    public void UseBullet()
    {
        if (bullets > 0)
            bullets--;
    }

    public void LoadBullet() => bullets++;

    public bool IsEmpty() => bullets <= 0;
    public bool IsOnGrab => isOnGrab;


    public void SetPhysicsEnabled(bool enabled)
    {
        if (rb != null)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
        }


        if (cols != null)
        {
            cols.SetActive(enabled);
        }

        Debug.Log($"physics : {enabled}");
    }

}
