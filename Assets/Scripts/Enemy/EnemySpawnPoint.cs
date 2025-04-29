using UnityEngine;

[ExecuteInEditMode]
public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _defaultSprite;

    private GameObject _enemyPrefabBuffer = null;
    private GameObject _enemy;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LevelManager.OnWorldReset += RespawnEnemy;
    }
    private void OnDisable()
    {
        LevelManager.OnWorldReset -= RespawnEnemy;
    }
    private void UpdateSprite()
    {
        if (_spriteRenderer == null) return;
        if(_enemyPrefab == null)
        {
            _spriteRenderer.sprite = _defaultSprite;
        }
        else
        {
            _spriteRenderer.sprite = _enemyPrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
    private void Update()
    {
        _spriteRenderer.enabled = !Application.isPlaying;
        if(_enemyPrefab != _enemyPrefabBuffer)
        {
            UpdateSprite();
            _enemyPrefabBuffer = _enemyPrefab;
        }
    }
    public void RespawnEnemy()
    {
        if(_enemy != null)
        {
            Destroy(_enemy);
        }
        _enemy = EnemyFactory.Instance.SpawnEnemy(_enemyPrefab, transform.position);
    }
}
