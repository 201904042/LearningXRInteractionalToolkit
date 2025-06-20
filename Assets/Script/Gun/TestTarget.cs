using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    [Header("색 변화 설정")]
    public Color hitColor = Color.red;
    public float fadeDuration = 2f;

    private Renderer rend;
    private Color originalColor;
    private float lerpTime;
    private bool isFading;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    private void Update()
    {
        if (!isFading)
            return;

        lerpTime += Time.deltaTime / fadeDuration;
        rend.material.color = Color.Lerp(hitColor, originalColor, lerpTime);

        if (lerpTime >= 1f)
            isFading = false;
    }

    private void TriggerHit()
    {
        // 색 즉시 변경
        rend.material.color = hitColor;
        lerpTime = 0f;
        isFading = true;
    }

    public void ApplyDamage(float Damage)
    {
        TriggerHit();
    }
}
