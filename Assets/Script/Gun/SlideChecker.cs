using System;
using UnityEngine;
using UnityEngine.Events;

public class SlideChecker : MonoBehaviour
{
    public enum CheckAxis { X, Y, Z }

    [Header("슬라이더 설정")]
    public Transform sliderTransform;           // 움직이는 슬라이더
    public Transform backLimitTransform;        // 후진 최대 위치 (최대 당김 위치)

    [Header("축 설정")]
    public CheckAxis axis = CheckAxis.Z;

    [Header("이벤트 설정")]
    public UnityEvent onSliderFullyBack;

    [Header("재장전 트리거 비율")]
    public float threshold = 0.2f;


    private bool hasTriggered = false;
    private float totalDistance = 0f;
    private Vector3 initialSliderPosition;

    void Start()
    {
        if (sliderTransform == null || backLimitTransform == null)
        {
            enabled = false;
            return;
        }

        initialSliderPosition = sliderTransform.position;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(sliderTransform.position, initialSliderPosition);
        if (!hasTriggered && dist < threshold)
        {
            onSliderFullyBack?.Invoke();
            hasTriggered = true;
        }
        else if (hasTriggered && dist >= threshold)
        {
            hasTriggered = false;
            this.enabled = false;
        }
    }

}
