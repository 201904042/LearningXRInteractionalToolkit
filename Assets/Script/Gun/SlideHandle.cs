using UnityEngine;

public class SlideHandle : MonoBehaviour
{
    public Animator animator;
    public Transform slideHandle;  // 잡히는 슬라이드
    public Transform slideStart;   // 기준 위치
    public float maxSlideDistance = 0.1f; // 최대 당길 수 있는 거리

    private bool isGrabbing = false;

    void Update()
    {
        if (isGrabbing)
        {
            float distance = Vector3.Distance(slideStart.position, slideHandle.position);
            float normalized = Mathf.Clamp01(distance / maxSlideDistance);
            animator.Play("Pistol_SlideBack", 0, normalized); // 애니메이션을 위치 기반으로 재생
            animator.speed = 0; // 애니메이션 시간은 코드로 제어
        }
    }

    public void OnGrabStart()
    {
        isGrabbing = true;
    }

    public void OnGrabEnd()
    {
        isGrabbing = false;
        animator.Play("Pistol_Idle");
        animator.speed = 1;
    }
}
