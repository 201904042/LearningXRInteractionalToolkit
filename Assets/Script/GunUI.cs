using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    [Header("ī�޶� Ÿ�� (���� �� �ڵ� ����)")]
    public Transform targetCamera;

    public TextMeshProUGUI text;

    void Start()
    {
        // �ڵ����� ����ī�޶� ����
        if (targetCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                targetCamera = cam.transform;
        }

        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        if (targetCamera == null)
            return;

        transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.position);
    }

    public void SetText(int curBullet)
    {
        SetText($"{curBullet}");
    }

    public void SetText(string t)
    {
        if (text == null)
            return;
        text.text = t;
    }
}
