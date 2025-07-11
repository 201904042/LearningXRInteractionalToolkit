using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject m_objectPrefab;
    [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private float m_spawnDelay = 3f;

    private GameObject m_currentObject;

    private void Start()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        m_currentObject = Instantiate(m_objectPrefab, m_spawnPoint.position, m_spawnPoint.rotation);

        XRGrabInteractable grab = m_currentObject.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectExited.AddListener(OnObjectGrabbed);
        }
        else
        {
            Debug.LogWarning($"{m_objectPrefab.name}���� XRGrabInteractable�� �����ϴ�.");
        }
    }

    private void OnObjectGrabbed(SelectExitEventArgs args)
    {
        // ������Ʈ�� ����ڰ� ������ ���ο� ������Ʈ�� ���� �ð� �� ����
        if (args.interactorObject != null)
        {
            StartCoroutine(SpawnAfterDelay());
        }

        if (m_currentObject.TryGetComponent(out XRGrabInteractable grab))
        {
            grab.selectExited.RemoveListener(OnObjectGrabbed);
        }
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(m_spawnDelay);
        SpawnObject();
    }
}

