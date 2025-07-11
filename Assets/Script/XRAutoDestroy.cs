using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class XRAutoDestroy : MonoBehaviour
{
    [Header("����")]
    public float timeToLive = 10f; // ������ ������ �� �� �� �ı�

    private XRGrabInteractable m_grabInteractable;
    private Coroutine m_destructionCoroutine;

    private void Awake()
    {
        m_grabInteractable = GetComponent<XRGrabInteractable>();

        if (m_grabInteractable == null)
        {
            Debug.LogWarning("XRGrabInteractable�� �����ϴ�.");
            enabled = false;
            return;
        }

        m_grabInteractable.selectEntered.AddListener(OnGrabbed);
        m_grabInteractable.selectExited.AddListener(ReleaseGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (m_destructionCoroutine != null)
        {
            StopCoroutine(m_destructionCoroutine);
            m_destructionCoroutine = null;
        }
    }

    private void ReleaseGrabbed(SelectExitEventArgs args)
    {
        m_destructionCoroutine = StartCoroutine(AutoDestructTimer());
    }

    private IEnumerator AutoDestructTimer()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_grabInteractable != null)
        {
            m_grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}
