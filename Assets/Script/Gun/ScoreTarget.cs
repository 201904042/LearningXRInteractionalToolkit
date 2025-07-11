using UnityEngine;

public class ScoreTarget : MonoBehaviour, IDamageable
{
    [Tooltip("�� Zone�� ����")]
    public int scoreValue = 0;

    public void ApplyDamage(float Damage)
    {
        Debug.Log("�Ѿ� ����!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("�Ѿ� ����!");
        }
    }
}
