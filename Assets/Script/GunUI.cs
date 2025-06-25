using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    [Header("카메라 타겟 (생략 시 자동 설정)")]
    public Transform targetCamera;

    public TextMeshProUGUI text;

    void Start()
    {
        // 자동으로 메인카메라 설정
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

        // UI가 항상 카메라를 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.position);
    }

    public void SetText(int curBullet)
    {
        SetText($"{curBullet}");
    }
    public void SetText(string s)
    {
        text.text = s;
    }
}
