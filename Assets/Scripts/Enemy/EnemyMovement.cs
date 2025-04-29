using UnityEngine;

[RequireComponent (typeof(BaseEnemy))]
public class EnemyMovement : MonoBehaviour
{
    private Vector2 _target;
    private Rigidbody2D _rb;
    private bool _isMoving = true;
    private BaseEnemy _enemy;
    private float _speed;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _target = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<BaseEnemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //Debug.Log(_isMoving);
        if(_isMoving) {
            SetMovement();
        }
        else {
            StopMovement();
        }
    }
    
    private void SetMovement()
    {
        Vector2 dir = _target - _enemy.GetPosition();
        dir.Normalize();
        if (dir == Vector2.zero)
        {
            StopMovement();
        }
        else
        {
            dir *= _speed;
            //Debug.Log(dir);
            _rb.linearVelocity = dir;
        }
        if(_rb.linearVelocityX > 0) _spriteRenderer.flipX = false;
        if(_rb.linearVelocityX < 0) _spriteRenderer.flipX = true;
    }
    private void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void EnableMovement(bool enabled)
    {
        _isMoving = enabled;
    }
    public void SetTarget(Vector2 target)
    {
        _target = target;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
