using static Enums;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class GunObject : MonoBehaviour
{
    [SerializeField] private GunFireManager m_fire;
    [SerializeField] private GunAuidioManager m_audio;
    [SerializeField] private GunMagazineManager m_mag;
    [SerializeField] private GunUIManager m_ui;
    [SerializeField] private Animator m_animator;
    private GunState m_state;

    public GunFireManager FireSystem { get => m_fire; }

    public GameObject SnapVolumn;
    

    public void OnGrab()
    {
        m_ui.SetUIActive(true);
        UpdateUI();

        SnapVolumn.SetActive(false);
    }

    public void ReleaseGrab()
    {
        m_ui.SetUIActive(false);
        SnapVolumn.SetActive(true);
    }

    private void Start()
    {
        m_state = GunState.NoMag;
    }

    public void Shoot()
    {
        if (m_state != GunState.Ready || !m_mag.HasAmmo()) return;

        m_mag.UseAmmo();
        m_fire.Fire();
        PlayFireAnim();

        StartCoroutine(DelayNextShot());
        UpdateUI();
    }

    [ContextMenu("Eject Magazine")]
    public void EjectMagazine(SelectExitEventArgs args)
    {
        Debug.Log("ÅºÃ¢ ºÐ¸®µÊ");
        m_mag.Eject();
        m_state = GunState.NoMag;
        UpdateUI();
    }

    [ContextMenu("Insert Magazine")]
    public void InsertMagazine(SelectEnterEventArgs args)
    {
        Debug.Log("ÅºÃ¢ ÀåÂøµÊ");
        Magazine inserted = args.interactableObject.transform.GetComponent<Magazine>();
        m_mag.Insert(inserted);
        m_state = GunState.NoSlide;
        UpdateUI();
    }

    public void Reload()
    {
        if (!m_mag.MagInserted())
        {
            m_state = GunState.NoMag;
        }
        else
        {
            if (m_mag.HasAmmo())
            {
                m_state = GunState.Ready;
            }
            else
            {
                m_state = GunState.NoAmmo;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        int count = m_mag.CurrentMag?.BulletCount ?? 0;
        m_ui.UpdateUI(m_state, count);
    }

    private void PlayFireAnim() => m_animator.SetTrigger("Fire");

    private IEnumerator DelayNextShot()
    {
        m_state = GunState.Delay;
        yield return new WaitForSeconds(m_fire.gunData.GetFireRate());
        m_state = m_mag.HasAmmo() ? GunState.Ready : GunState.NoAmmo;
        UpdateUI();
    }

   
}
