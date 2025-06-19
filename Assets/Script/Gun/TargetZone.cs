using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [Tooltip("�� Zone�� ����")]
    public int scoreValue = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log($"{gameObject.name}�� ����! ����: {scoreValue}");
            // �ʿ� ��: ������ GameManager � ���� ����
        }
    }
}
