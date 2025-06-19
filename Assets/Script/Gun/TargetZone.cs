using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [Tooltip("이 Zone의 점수")]
    public int scoreValue = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log($"{gameObject.name}에 명중! 점수: {scoreValue}");
            // 필요 시: 점수를 GameManager 등에 전달 가능
        }
    }
}
