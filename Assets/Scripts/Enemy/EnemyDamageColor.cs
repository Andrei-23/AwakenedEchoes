using System.Collections;
using UnityEngine;

public class EnemyDamageColor : MonoBehaviour
{
    [SerializeField] private Color _damageColor;
    private SpriteRenderer _spriteRenderer;
    private float _timer = 10f;
    private float _hitTime = 0.3f;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        float a = Mathf.Clamp01(_timer / _hitTime);
        _spriteRenderer.color = Color.white * a + _damageColor * (1f - a);
    }
    public void Hit()
    {
        //_spriteRenderer.color = _damageColor;
        _timer = 0f;
    }
}
