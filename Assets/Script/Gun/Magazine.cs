using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Magazine : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Collider[] childColliders;

    [SerializeField] private int gunId;
    [SerializeField] private int bullets;
    public bool autoInit = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();

        if (autoInit)
            Init(0, 10);
    }

    public void Init(int id, int num)
    {
        this.gunId = id;
        bullets = num;
        SetGrabbable(true);
    }

    public void SetGrabbable(bool tf)
    {
        grabInteractable.enabled = tf;
    }

    public void SetBullets(int num)
    {
        bullets = num;
    }

    public int GetId()
    {
        return gunId;
    }

    public int GetBullets()
    {
        return bullets;
    }

    public void UseBullet()
    {
        bullets--;
    }

    public void LoadBullet()
    {
        bullets++;
    }

    public bool IsEmpty()
    {
        return bullets == 0;
    }

    public void SetPhysicsEnabled(bool enabled)
    {
        if (rb != null)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
        }

        if (childColliders != null)
        {
            foreach (var col in childColliders)
            {
                col.enabled = enabled;
            }
        }
    }
}
