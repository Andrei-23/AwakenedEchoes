using UnityEngine;

public class PlayerHitCollider : MonoBehaviour
{
    
    public void Hit(int damage)
    {
        PlayerStats.Instance.TakeDamage(damage);
    }
}
