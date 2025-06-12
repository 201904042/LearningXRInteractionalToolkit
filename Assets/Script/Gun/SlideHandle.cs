using UnityEngine;

public class SlideHandle : MonoBehaviour
{
    public Animator animator;
    public Transform slideHandle;  // ������ �����̵�
    public Transform slideStart;   // ���� ��ġ
    public float maxSlideDistance = 0.1f; // �ִ� ��� �� �ִ� �Ÿ�

    private bool isGrabbing = false;

    void Update()
    {
        if (isGrabbing)
        {
            float distance = Vector3.Distance(slideStart.position, slideHandle.position);
            float normalized = Mathf.Clamp01(distance / maxSlideDistance);
            animator.Play("Pistol_SlideBack", 0, normalized); // �ִϸ��̼��� ��ġ ������� ���
            animator.speed = 0; // �ִϸ��̼� �ð��� �ڵ�� ����
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
