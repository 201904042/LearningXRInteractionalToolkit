using UnityEngine;

public class ScoreTarget : MonoBehaviour, IDamageable
{
    [Tooltip("이 Zone의 점수")]
    public int scoreValue = 0;

    public void ApplyDamage(float Damage)
    {
        Debug.Log("총알 적중!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("총알 적중!");
        }
    }
}
