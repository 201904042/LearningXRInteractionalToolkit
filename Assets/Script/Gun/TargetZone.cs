using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [Tooltip("�� Zone�� ����")]
    public int scoreValue = 0;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("�Ѿ� ����!");
        }
    }
}
