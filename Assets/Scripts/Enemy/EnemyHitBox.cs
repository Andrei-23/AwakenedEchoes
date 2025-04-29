using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField] private BaseEnemy _enemy;

    public void Hit(float damage)
    {
        _enemy.TakeDamage(damage);
    }
}
