using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private LayerMask _playerMask;
    private void SetSpawn()
    {
        Debug.Log("Checkpoint!");
        PlayerStats.Instance.SetRespawnPoint(_spawnPoint.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & _playerMask) == _playerMask)
        {
            SetSpawn();
        }
    }
}
